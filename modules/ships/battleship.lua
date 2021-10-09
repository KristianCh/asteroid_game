function battleship_init(self) 
	self.armor = 5 * self.level
	self.main_cooldown_time = 3 - (self.level-1) / 2
	self.main_cooldown = self.main_cooldown_time
	self.small_cooldown = 0.25
	self.charges = 2 + self.level
	self.charges_reload = self.charges
	self.projectile_speed = 750
	self.damage = 40 + self.level * 20
	self.max_health = 100 + (self.level-1) * 50
	self.health = self.max_health
	self.class_1 = hash("physical")
end

function battleship_update(self, dt)
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

local function battleship_target(self, target) 
	if target.found then
		self.charges = self.charges - 1
		self.small_cooldown = 0.25
		local vec_to_target = vmath.normalize(go.get_position(target.enemy) - go.get_position() + go.get(target.enemy, "velocity") * (vmath.length(go.get_position(target.enemy) - go.get_position()) / self.projectile_speed))
		factory.create("/manager#player_projectile_factory", go.get_position(), nil, {speed = self.projectile_speed, heading = vec_to_target, damage = self.damage}, vmath.vector3(0.5))
		if self.charges == 0 then
			self.main_cooldown = self.main_cooldown_time * self.cooldown_mult
			self.small_cooldown = 0.25
			self.charges = self.charges_reload
		end
	end
end

function battleship_message(self, message_id, message, sender) 
	if message_id == hash("target_enemy_response") then
		if message.found then
			battleship_target(self, message)
		end
	elseif message_id == hash("post_init_ready") then
		if self.is_flagship then
			msg.post("game:/manager", "apply_status_to_fleet", 
			{
				type = "property", property_name = "armor", value = 5, mix = "additive"
			})
		end
	end
end