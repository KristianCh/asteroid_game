function damage_aoe_init(self)
	self.rot = 0
	
	self.update_type = damage_aoe_update
	self.message_type = damage_aoe_message

	go.set("#sprite", "tint", vmath.vector4(1, 0.25, 0.25, 1))
end

function damage_aoe_update(self, dt) 
	msg.post("/manager", "target_all_enemies", {dt = dt})
	
	self.rot = self.rot + dt * 30
	local rot_z = vmath.quat_rotation_z(math.rad(self.rot * dt))
	local current_rot = go.get_rotation()
	go.set_rotation(current_rot * rot_z)
end

function damage_aoe_message(self, message_id, message, sender)
	if message_id == hash("target_enemy_response") then
		for key, v in ipairs(message.enemies) do
			local asteroid_url = v
			local asteroid_position = go.get_position(asteroid_url)
			local vec_to_asteroid = asteroid_position - go.get_position()
			local distance = vmath.length(vec_to_asteroid) - 10 * go.get(asteroid_url, "size")
			if distance <= self.range then
				msg.post(asteroid_url, "damage_asteroid", {damage = self.damage * message.dt, type = "energy"})
			end
		end
	end
end