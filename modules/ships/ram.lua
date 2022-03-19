function ram_init(self) 
	self.cooldown = 0.25
	self.cooldown_time = 0.25
	self.speed = 400
	self.armor = 20 + 10 * self.level
	self.max_health = 150 + (self.level-1) * 100
	self.health = self.max_health
	self.base_explosion_damage = 50 + 50 * self.level
	self.explosion_mult = self.level
	self.explosion_range = 0.75 + (self.level - 1) * 0.25
	self.class_1 = hash("physical")

	self.update_type = ram_update
	self.message_type = ram_message

	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 0})
end

function ram_update(self, dt)
	if self.cooldown > 0 then 
		self.cooldown = self.cooldown - dt
	end
	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 1 - self.cooldown / self.cooldown_time * self.cooldown_mult})
end

function ram_message(self, message_id, message, sender)
	if message_id == hash("contact_point_response") then
		if message.other_group == hash("asteroid") and self.cooldown <= 0 then
			local enemy_pos = go.get_position(message.other_id)
			local angle_cos = vmath.dot(self.heading, vmath.normalize(go.get_position() - enemy_pos))
			if angle_cos < 0.2 then
				self.cooldown = self.cooldown_time
				local damage = self.base_explosion_damage + (vmath.length(message.relative_velocity) / 10) * self.explosion_mult
				msg.post( "/manager", "create_player_aoe", 
					{
						position = enemy_pos, 
						rotation = vmath.quat_rotation_z(math.random(-math.pi, math.pi)),
						properties = {duration = 0.5, damage = damage, type = 3}, 
						scale = vmath.vector3(self.explosion_range)
					}
				)
			end
		end
	elseif message_id == hash("post_init_ready") then
		if self.is_flagship then
			msg.post("game:/manager", "apply_status_to_fleet", 
			{
				type = "property", property_name = "armor", value = 10, mix = "add"
			})
		end
	end
end