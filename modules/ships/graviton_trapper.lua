function graviton_trapper_init(self) 
	self.evasion = 2
	self.cooldown = 12
	self.cooldown_time = 12
	self.speed = 300
	self.aoe_duration = 2.5 + self.level * 2.5
	self.subscribed_count = 0
	self.max_subscribed = 2 + self.level
	self.subscribed = {}
	self.max_health = 75 + (self.level-1) * 25
	self.health = self.max_health
	self.class_1 = hash("energy")
	self.class_2 = hash("graviton")

	self.update_type = graviton_trapper_update
	self.message_type = graviton_trapper_message

	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 0})
end

function graviton_trapper_update(self, dt)
	if self.cooldown > 0 then 
		self.cooldown = self.cooldown - dt
	end
	if self.cooldown <= 0 then
		msg.post("/manager", "target_random_enemy", {dt = dt})
	end
	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 1 - self.cooldown / self.cooldown_time * self.cooldown_mult})
end

local function graviton_trapper_target(self, message)
	if message.found then
		local enemy_pos = go.get_position(message.enemy)
		if enemy_pos.x > 0 and enemy_pos.x < WIDTH and enemy_pos.y > 0 and enemy_pos.y < HEIGHT then
			self.cooldown = self.cooldown_time
			factory.create(
				"/manager#player_aoe_factory", enemy_pos, nil, 
				{duration = self.aoe_duration}, vmath.vector3(1)
			)
		end
	end
end

function graviton_trapper_message(self, message_id, message, sender)
	if message_id == hash("target_enemy_response") then
		if message.found then
			graviton_trapper_target(self, message)
		end
	elseif message_id == hash("post_init_ready") then
		if self.is_flagship then
			msg.post("game:/manager", "apply_status_to_fleet", 
			{
				type = "property", property_name = "impact_resistance", value = 0.9, mix = "multiply"
			})
		end
	end
end