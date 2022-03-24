require "modules.utils"

function firing_init(self)
	self.target_range = 1000
	go.set("mesh_container#mesh", "color", vmath.vector4(1, 0.333, 0.333, 1))
	self.color = vmath.vector4(1, 0.333, 0.333, 1)
	self.physical_damage_resistance = 1
	self.explosive_damage_resistance = 1
	self.energy_damage_resistance = 1
	self.init_timer = 3
	self.cooldown_time = 1
	self.cooldown = self.cooldown_time
	self.found_target = false
	self.heading = vmath.vector3(0, 0, 0)

	if self.size ~= 3 then
		msg.post("#co" .. self.size, "apply_force", {
			force = vmath.vector3(math.random(-1000, 1000), math.random(-1000, 1000), 0), 
			position = go.get_position()
		})
	end
end

function firing_update(self, dt)
	if self.init_timer > 0 then 
		self.init_timer = self.init_timer - dt
	elseif self.cooldown > 0 then
		self.cooldown = self.cooldown - dt
	end

	self.heading = vmath.lerp(dt, self.heading, vmath.vector3(0))
	go.set_position(go.get_position() + self.heading * dt)
	
	msg.post("/manager", "target_closest_ship", {pos = go.get_position(), range = 10000, dt = dt})
end

function firing_message_handling(self, message_id, message, sender) 
	if message_id == hash("target_ship_response") then
		if message.found then
			local vec_to_ship = go.get_position(message.ship) - go.get_position()
			local dist = vmath.length(vec_to_ship)
			if not self.found_target and self.init_timer <= 0 and dist < self.target_range and is_in_game_world(go.get_position()) then
				self.found_target = true
				self.heading = go.get("#co" .. self.size, "linear_velocity")
				msg.post("#co" .. self.size, "disable")
				msg.post("#co" .. self.size .. "_kinematic", "enable")
			elseif self.cooldown <= 0 then
				self.cooldown = self.cooldown_time
				factory.create("/manager#asteroid_projectile_factory", go.get_position() + vmath.normalize(vec_to_ship) * self.size * 50, nil, {speed = 750, heading = vmath.normalize(vec_to_ship), damage = 25}, vmath.vector3(0.5))
			end
		end
	end
end