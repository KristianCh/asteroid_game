function gravity_tether_init(self) 
	self.evasion = 2
	self.damage = 50 + 25 * self.level * self.level
	self.slowdown = 10 * self.level
	self.dt = 1/60
	self.speed = 300
	self.max_health = 75 + (self.level-1) * 25
	self.health = self.max_health
	self.class_1 = hash("energy")
	self.class_2 = hash("graviton")

	self.update_type = gravity_tether_update
	self.message_type = gravity_tether_message

	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 0})
end

function gravity_tether_update(self, dt)
	self.dt = dt
end

function gravity_tether_message(self, message_id, message, sender)
	if message_id == hash("collision_response") and message.own_group == hash("ship_special_range") then
		local asteroid_url = msg.url("game", message.other_id, "base_asteroid")
		local asteroid_size = go.get(asteroid_url, "size")
		local asteroid_co_url = msg.url("game", message.other_id, "co" .. asteroid_size)
		local linear_velocity = go.get(asteroid_co_url, "linear_velocity")
		local asteroid_position = go.get_position(asteroid_url)

		local force_mult = 2 * self.level

		local range_falloff = ((1500 * go.get_scale().x) - vmath.length(asteroid_position - go.get_position())) / 1500 * go.get_scale().x
		range_falloff = math.max(0, range_falloff)
		
		msg.post("@render:", "draw_line", {start_point = go.get_position(), end_point = asteroid_position, color = vmath.vector4(0.5, 0.5, 1, 1)*range_falloff})
		msg.post(asteroid_co_url, "apply_force", {
			force = -linear_velocity * force_mult, 
			position = asteroid_position
		})
		msg.post(asteroid_url, "damage_asteroid", {damage = 10 * self.dt * (range_falloff + 0.5), type = "energy"})
	end
end