function missile_cruiser_init(self) 
	self.main_cooldown_time = 4 - self.level/2
	self.main_cooldown = self.main_cooldown_time
	self.small_cooldown = 0.25
	self.charges = 2 + self.level
	self.projectile_speed = 400
end

function missile_cruiser_update(self, dt)
	if self.main_cooldown > 0 then 
		self.main_cooldown = self.main_cooldown - dt
	end
	if self.small_cooldown > 0 then 
		self.small_cooldown = self.small_cooldown - dt
	end
	if self.small_cooldown <= 0 and self.main_cooldown <= 0 and self.charges > 0 then
		msg.post("/manager", "target_closest_enemy", {pos = go.get_position(), range = 10000, dt = dt})
	end
	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 1 - self.main_cooldown / self.main_cooldown_time})
end

function missile_cruiser_target(self, target) 
	if target.found then
		self.charges = self.charges - 1
		self.small_cooldown = 0.25
		local vec_to_target = vmath.normalize(go.get_position(target.enemy) - go.get_position())
		factory.create("/manager#player_projectile_factory", go.get_position(), nil, 
		{speed = self.projectile_speed, heading = vec_to_target, damage = 30 + self.level*20, 
		target = target.enemy, type = 2}, vmath.vector3(0.5))
		if self.charges == 0 then
			self.main_cooldown = self.main_cooldown_time
			self.small_cooldown = 0.25
			self.charges = 2 + self.level
		end
	end
end