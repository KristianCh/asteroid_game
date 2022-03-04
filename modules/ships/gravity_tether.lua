function gravity_tether_init(self) 
	self.evasion = 2
	self.damage = 50 + 25 * self.level * self.level
	self.slowdown = 70 - self.level * 10
	self.dt = 1/60
	self.speed = 300
	self.range = 300
	self.subscribed_count = 0
	self.max_subscribed = 2 + self.level
	self.subscribed = {}
	self.max_health = 75 + (self.level-1) * 25
	self.health = self.max_health
	self.class_1 = hash("energy")
	self.class_2 = hash("graviton")

	self.update_type = gravity_tether_update
	self.message_type = gravity_tether_message

	msg.post(self.stat_tracker, "set_cooldown", {cooldown = 0})

	if self.is_flagship then
		self.range = self.range * 1.75
	end
end

function gravity_tether_update(self, dt)
	self.dt = dt

	msg.post("/manager", "target_k_closest_enemies", {pos = go.get_position(), range = self.range, dt = dt, k = self.max_subscribed})

	--[[if self.subscribed_count < self.max_subscribed then
		msg.post("/manager", "target_closest_enemy", {pos = go.get_position(), range = self.range, dt = dt})
	end]]--

	--[[for key, asteroid_url in ipairs(self.subscribed) do
		local asteroid_size = go.get(asteroid_url, "size")
		local asteroid_position = go.get_position(asteroid_url)
		local force_mult = 2 * self.level

		local range_falloff = (self.range - vmath.length(asteroid_position - go.get_position())) / self.range
		range_falloff = math.max(0, range_falloff)
		print(range_falloff)

		msg.post("@render:", "draw_line", {start_point = go.get_position(), end_point = asteroid_position, color = vmath.vector4(0.5, 0.5, 1, 1)*range_falloff})
		msg.post(asteroid_url, "graviton_effect", {
			slowdown = self.slowdown,
			co = "#co" .. asteroid_size,
			force_mult = force_mult * dt
		})
		msg.post(asteroid_url, "damage_asteroid", {damage = 10 * self.dt * (range_falloff + 0.5), type = "energy"})
		if vmath.length(asteroid_position - go.get_position()) > self.range then
			self.subscribed_count = self.subscribed_count - 1
			msg.post(asteroid_url, "subscribe")
			table.remove(self.subscribed, key)
		end	
	end]]--
end

local function gravity_tether_target(self, target) 
	--[[if target.found and self.subscribed_count < self.max_subscribed then
		msg.post(target.enemy, "subscribe")
		self.subscribed_count = self.subscribed_count + 1
		table.insert(self.subscribed, target.enemy)
	end]]--
	for key, v in ipairs(target.enemies) do
		local asteroid_url = v[1]
		local distance = v[2]
		local asteroid_size = go.get(asteroid_url, "size")
		local asteroid_position = go.get_position(asteroid_url)
		local force_mult = 2 * self.level

		local range_falloff = (self.range - distance) / self.range
		range_falloff = math.max(0, range_falloff)
		
		msg.post("@render:", "draw_line", {start_point = go.get_position(), end_point = asteroid_position, color = vmath.vector4(0.5, 0.5, 1, 1)*range_falloff})
		msg.post(asteroid_url, "graviton_effect", {
			slowdown = self.slowdown,
			co = "#co" .. asteroid_size,
			force_mult = force_mult * target.dt
		})
		msg.post(asteroid_url, "damage_asteroid", {damage = 10 * target.dt * (range_falloff + 0.5), type = "energy"})
	end
end

function gravity_tether_message(self, message_id, message, sender)
	--[[if message_id == hash("unsubscribe") then
		print("unsub", sender)
		self.subscribed_count = self.subscribed_count - 1
		for key, value in pairs(self.subscribed) do
			if sender == value then
				table.remove(self.subscribed, key)
				break
			end
		end
	else]]if message_id == hash("target_enemy_response") then
		if message.found then
			gravity_tether_target(self, message)
		end
	end
end