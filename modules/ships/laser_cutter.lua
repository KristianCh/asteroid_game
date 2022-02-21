function laser_cutter_init(self) 
	self.target_offset = vmath.vector3(0, 0, 0)
	self.evasion = 2 + self.level * 0.25
	self.damage = 50 + 25 * self.level * self.level
	self.speed = 300 + self.level * 50
	self.max_health = 75 + (self.level-1) * 25
	self.health = self.max_health
	self.targeting_range = 350 + self.level * 150
	self.class_1 = hash("energy")

	self.update_type = laser_cutter_update
	self.message_type = laser_cutter_message
end

function laser_cutter_update(self, dt)
	self.target_offset = vmath.lerp(dt, self.target_offset, vmath.vector3(0, 0, 0))
	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 0})
	msg.post("/manager", "target_closest_enemy", {pos = go.get_position(), range = self.targeting_range, dt = dt})
end

local function laser_cutter_target(self, target) 
	local range_falloff = 1
	if target.found then
		self.target_offset = vmath.lerp(target.dt * 30, self.target_offset, go.get_position(target.enemy) - go.get_position())
		local target_pos = go.get_position() + self.target_offset
		range_falloff = math.sqrt(1 - (vmath.length(go.get_position(target.enemy) - go.get_position()) / self.targeting_range))
		msg.post(self.stat_tracker, "set_cooldown", {cooldown = range_falloff})
		msg.post(target.enemy, "damage_asteroid", {damage = self.damage * target.dt * range_falloff, type = "energy"})
		local c = 1 * math.pow(range_falloff, 3)
		msg.post("@render:", "draw_line", {start_point = go.get_position(), end_point = go.get_position() + self.target_offset, color = vmath.vector4(c*2, c/3, c/3, 1)})
	end
end

function laser_cutter_message(self, message_id, message, sender) 
	if message_id == hash("target_enemy_response") then
		if message.found then
			laser_cutter_target(self, message)
		end
	elseif message_id == hash("post_init_ready") then
		if self.is_flagship then
			msg.post("game:/manager", "apply_status_to_fleet", 
			{
				type = "property", property_name = "cooldown_mult", value = 0.9, mix = "multiply", class_mask = hash("guided")
			})
		end
	end
end