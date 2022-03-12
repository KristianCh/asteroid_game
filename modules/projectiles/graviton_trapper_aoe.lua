function graviton_trapper_aoe_init(self)
	self.rot = 0
	
	self.update_type = graviton_trapper_aoe_update
	self.message_type = graviton_trapper_aoe_message

	go.set("#sprite", "tint", vmath.vector4(0.25, 0.75, 1, 1))
end

function graviton_trapper_aoe_update(self, dt) 
	msg.post("/manager", "target_all_enemies", {dt = dt})
	
	self.rot = self.rot + dt * 30
	local rot_z = vmath.quat_rotation_z(math.rad(self.rot * dt))
	local current_rot = go.get_rotation()
	go.set_rotation(current_rot * rot_z)
end

function graviton_trapper_aoe_message(self, message_id, message, sender)
	for key, v in ipairs(message.enemies) do
		local asteroid_url = v
		local asteroid_position = go.get_position(asteroid_url)
		local vec_to_asteroid = asteroid_position - go.get_position()
		local distance = vmath.length(vec_to_asteroid) - 10 * go.get(asteroid_url, "size")
		if distance <= self.range then
			local force = -vmath.normalize(vec_to_asteroid) * 7500
			msg.post(asteroid_url, "apply_force", {
				force = force,
				position = asteroid_position
			})
		end
	end
end