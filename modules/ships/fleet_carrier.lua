function fleet_carrier_init(self) 
	self.armor = 5 * self.level
	self.cooldown_time = 2
	self.cooldown = self.cooldown_time
	self.drone_damage = 10 + 5 * self.level
	self.current_drones = 0
	self.drone_range = 500
	self.max_drones = 2 + self.level

	self.max_health = 100 + (self.level-1) * 50
	self.health = self.max_health
	self.class_1 = hash("physical")
	self.class_1 = hash("carrier")

	self.update_type = fleet_carrier_update
	self.message_type = fleet_carrier_message
end

function fleet_carrier_update(self, dt)
	if self.cooldown > 0 then 
		self.cooldown = self.cooldown - dt
	end
	
	if self.cooldown <= 0 and self.current_drones < self.max_drones + self.drone_bonus then
		msg.post("/manager", "create_player_drone", {
			position = go.get_position(), 
			properties = {heading = -self.heading, damage = self.drone_damage, mothership_url = go.get_id()}, 
			scale = vmath.vector3(0.1)
		})
		self.current_drones = self.current_drones + 1
		self.cooldown = self.cooldown_time
	end
	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 1 - self.cooldown / self.cooldown_time * self.cooldown_mult})
end

local function fleet_carrier_target(self, target) 
	msg.post(target.dt, "recieve_target", target)
end

function fleet_carrier_message(self, message_id, message, sender) 
	if message_id == hash("target_enemy_response") then
		msg.post(message.dt, "recieve_target", message)
	elseif message_id == hash("request_target") then
		msg.post("/manager", "target_closest_enemy", {pos = go.get_position(), range = self.drone_range, dt = sender})
	elseif message_id == hash("drone_destroyed") then
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