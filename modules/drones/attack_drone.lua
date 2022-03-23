require "modules.utils"

function attack_drone_init(self)

	self.update_type = attack_drone_update
	self.message_type = attack_drone_message

	self.cooldown = 1
	self.cooldown_time = 1
	
	self.projectile_speed = 750

	self.mothership_type = "fleet_carrier"
	go.set("#model", "tint", vmath.vector4(1, 0.33, 0.33, 1))
end

function attack_drone_update(self, dt)
	msg.post("/manager", "get_special_ship_url", {type = self.mothership_type})
	
	local pos = go.get_position()
	local vec_to_target = vmath.normalize(self.target_position - pos)
	self.heading = vmath.normalize(vmath.slerp(self.evasion * dt, self.heading, vec_to_target))

	if self.cooldown > 0 then 
		self.cooldown = self.cooldown - dt
	end
end

function attack_drone_message(self, message_id, message, sender)
	if message_id == hash("return_special_ship_url") then
		if message.url ~= nil then
			msg.post(message.url, "request_target")
		else
			msg.post(".", "self_destruct")
		end
	elseif message_id == hash("recieve_target") then
		self.target_position = message.target_position
		if message.found then
			local enemy_size = go.get(message.enemy, "size")
			local pos = go.get_position()
			self.target_position = message.target_position - vmath.normalize(message.target_position - pos) * 50 * enemy_size
			if self.cooldown <= 0 then
				self.cooldown = self.cooldown_time
				local fire_pos = vmath.normalize(message.target_position - pos + go.get(message.enemy, "average_current_velocity") * (vmath.length(message.target_position - pos) / self.projectile_speed))
				factory.create("/manager#player_projectile_factory", pos, nil, {speed = self.projectile_speed, heading = fire_pos, damage = self.damage}, vmath.vector3(0.2))
			end
		end
	end
end