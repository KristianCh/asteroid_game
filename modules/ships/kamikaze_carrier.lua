function kamikaze_carrier_init(self) 
	self.armor = 5 * self.level
	self.cooldown_time = 1
	self.cooldown = self.cooldown_time
	self.drone_damage = 40 + 40 * self.level
	self.current_drones = 0
	self.drone_range = 500
	self.drone_aoe_range = 0.5 + (self.level-1) * 0.125
	self.max_drones = 2 + self.level

	self.max_health = 100 + (self.level-1) * 50
	self.health = self.max_health
	self.class_1 = hash("nuker")
	self.class_1 = hash("carrier")

	self.current_target = nil

	self.update_type = kamikaze_carrier_update
	self.message_type = kamikaze_carrier_message

	msg.post("/manager", "add_special_ship_url", {type = "kamikaze_carrier"})
end

function kamikaze_carrier_final(self)
	msg.post("/manager", "remove_special_ship_url", {type = "kamikaze_carrier"})
end

function kamikaze_carrier_update(self, dt)
	self.current_target = nil
	msg.post("/manager", "target_closest_enemy", {pos = go.get_position(), range = self.drone_range, dt = dt})
	
	if self.cooldown > 0 then 
		self.cooldown = self.cooldown - dt
	end

	if self.health <= 0 then
		msg.post("/manager", "remove_special_ship_url", {type = "kamikaze_carrier"})
	end
	
	if self.cooldown <= 0 and self.current_drones < self.max_drones + self.drone_bonus then
		msg.post("/manager", "create_player_drone", {
			position = go.get_position(), 
			properties = {
				type = 2, heading = -self.heading, damage = self.drone_damage, 
				aoe_range = self.drone_aoe_range
			}, 
			scale = vmath.vector3(0.1)
		})
		self.current_drones = self.current_drones + 1
		self.cooldown = self.cooldown_time
	end
	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 1 - self.cooldown / self.cooldown_time * self.cooldown_mult})
end

local function kamikaze_carrier_target(self, target)
	
end

function kamikaze_carrier_message(self, message_id, message, sender) 
	if message_id == hash("target_enemy_response") then
		if message.found then
			self.current_target = message.enemy
		end
	elseif message_id == hash("request_target") then
		local tp = go.get_position()
		local found = self.current_target ~= nil
		if found then
			tp = go.get_position(self.current_target)
		end
		msg.post(sender, "recieve_target", {found = found, enemy = self.current_target, message.dt, target_position = tp})
	elseif message_id == hash("alert_special_ship") then
		self.cooldown = self.cooldown_time
		self.current_drones = self.current_drones - 1
	elseif message_id == hash("post_init_ready") then
		if self.is_flagship then
			msg.post("game:/manager", "apply_status_to_fleet", 
			{
				type = "property", property_name = "drone_bonus", value = 1, mix = "add"
			})
		end
	end
end