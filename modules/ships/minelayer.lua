function minelayer_init(self) 
	self.armor = 5 * self.level
	self.main_cooldown_time = 5 - (self.level-1) / 2
	self.main_cooldown = self.main_cooldown_time
	self.small_cooldown = 0.25
	self.charges = 1 + self.level
	self.charges_reload = self.charges
	self.damage = 20 + self.level * 40
	self.max_health = 100 + (self.level-1) * 50
	self.health = self.max_health
	self.speed = 200
	self.evasion = 1.75
	self.class_1 = hash("physical")

	self.update_type = minelayer_update
	self.message_type = minelayer_message
end

function minelayer_update(self, dt)
	if self.main_cooldown > 0 then 
		self.main_cooldown = self.main_cooldown - dt
	end
	if self.small_cooldown > 0 then 
		self.small_cooldown = self.small_cooldown - dt
	end
	if self.small_cooldown <= 0 and self.main_cooldown <= 0 and self.charges > 0 and go.get("/manager#manager", "enemy_count") > 0 then
		self.charges = self.charges - 1
		self.small_cooldown = 0.25
		factory.create("/manager#player_mine_factory", go.get_position(), nil, {
			heading = self.heading, value = self.damage, angular_velocities = vmath.vector3(self.heading.y, self.heading.x, 0) * 2
		}, vmath.vector3(0.5))
		if self.charges == 0 then
			self.main_cooldown = self.main_cooldown_time * self.cooldown_mult
			self.small_cooldown = 0.25
			self.charges = self.charges_reload
		end
	end
	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 1 - self.main_cooldown / self.main_cooldown_time * self.cooldown_mult})
end

local function minelayer_target(self, target) 

end

function minelayer_message(self, message_id, message, sender) 
	if message_id == hash("post_init_ready") then
		if self.is_flagship then
			msg.post("game:/manager", "apply_status_to_fleet", 
			{
				type = "property", property_name = "armor", value = 5, mix = "add"
			})
		end
	end
end