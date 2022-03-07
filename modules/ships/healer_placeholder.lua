function healer_placeholder_init(self) 
	self.main_cooldown_time = 4 - (self.level-1) / 2
	self.main_cooldown = self.main_cooldown_time
	self.small_cooldown = 0.05
	self.charges = 4 + self.level * 2
	self.charges_reload = self.charges
	self.projectile_speed = 750
	self.damage = 20
	self.max_health = 75 + (self.level-1) * 25
	self.health = self.max_health
	self.speed = 350
	self.class_1 = hash("support")

	self.update_type = healer_placeholder_update
	self.message_type = healer_placeholder_message
end

function healer_placeholder_update(self, dt)
	if self.main_cooldown > 0 then 
		self.main_cooldown = self.main_cooldown - dt
	end
	if self.small_cooldown > 0 then 
		self.small_cooldown = self.small_cooldown - dt
	end
	if self.small_cooldown <= 0 and self.main_cooldown <= 0 and self.charges > 0 then
		msg.post("/manager", "target_closest_enemy", {pos = go.get_position(), range = 10000, dt = dt})
	end
	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 1 - self.main_cooldown / self.main_cooldown_time * self.cooldown_mult})
end

local function healer_placeholder_target(self, target) 
	if target.found then
		self.charges = self.charges - 1
		self.small_cooldown = 0.05
		local vec_to_target = vmath.normalize(go.get_position(target.enemy) - go.get_position() + go.get(target.enemy, "velocity") * (vmath.length(go.get_position(target.enemy) - go.get_position()) / self.projectile_speed))
		factory.create("/manager#player_projectile_factory", go.get_position(), nil, {speed = self.projectile_speed, heading = vec_to_target, damage = self.damage, type = 3, level = self.level}, vmath.vector3(0.33))
		if self.charges == 0 then
			self.main_cooldown = self.main_cooldown_time * self.cooldown_mult
			self.small_cooldown = 0.05
			self.charges = self.charges_reload
		end
	end
end

function healer_placeholder_message(self, message_id, message, sender) 
	if message_id == hash("target_enemy_response") then
		if message.found then
			healer_placeholder_target(self, message)
		end
	elseif message_id == hash("post_init_ready") then
		if self.is_flagship then
			msg.post("game:/manager", "apply_status_to_fleet", 
			{
				type = "property", property_name = "max_health", value = 1.10, mix = "multiply"
			})
			msg.post("game:/manager", "apply_status_to_fleet", 
			{
				type = "property", property_name = "health", value = 1.10, mix = "multiply"
			})
		end
	end
end