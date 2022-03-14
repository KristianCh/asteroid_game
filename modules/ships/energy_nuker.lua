function energy_nuker_init(self) 
	self.evasion = 2
	self.cooldown = 12
	self.cooldown_time = 12
	self.speed = 300
	self.aoe_damage = 50 + 50 * self.level
	self.aoe_range = 0.5 + self.level * 0.5
	self.aoe_duration = 3 + self.level * 2
	self.max_health = 75 + (self.level-1) * 25
	self.health = self.max_health
	self.class_1 = hash("energy")
	self.class_2 = hash("nuker")

	self.update_type = energy_nuker_update
	self.message_type = energy_nuker_message

	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 0})
end

function energy_nuker_update(self, dt)
	if self.cooldown > 0 then 
		self.cooldown = self.cooldown - dt
	end
	if self.cooldown <= 0 then
		msg.post("/manager", "target_closest_enemy", {pos = go.get_position(), range = 10000, dt = dt})
	end
	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 1 - self.cooldown / self.cooldown_time * self.cooldown_mult})
end

local function energy_nuker_target(self, message)
	if message.found then
		local enemy_pos = go.get_position(message.enemy)
		if enemy_pos.x > 0 and enemy_pos.x < WIDTH and enemy_pos.y > 0 and enemy_pos.y < HEIGHT then
			self.cooldown = self.cooldown_time
			msg.post( "/manager", "create_player_aoe", 
				{
					position = enemy_pos, 
					properties = {duration = self.aoe_duration, damage = self.aoe_damage, type = 2}, 
					scale = vmath.vector3(self.aoe_range)
				}
			)
		end
	end
end

function energy_nuker_message(self, message_id, message, sender)
	if message_id == hash("target_enemy_response") then
		if message.found then
			energy_nuker_target(self, message)
		end
	elseif message_id == hash("post_init_ready") then
		if self.is_flagship then
			msg.post("game:/manager", "apply_status_to_fleet", 
			{
				affected_property = "energy_damage_mult", value = 1.25, mix = "multiply"
			})
		end
	end
end