function magnetic_init(self)
	go.set("mesh_container#mesh", "color", vmath.vector4(1, 0, 0, 1))
	self.color = vmath.vector4(1, 0, 0, 1)
	self.physical_damage_resistance = 0.8
	self.explosive_damage_resistance = 0.8
	self.energy_damage_resistance = 1.2
	go.animate("mesh_container#mesh", "color", go.PLAYBACK_LOOP_PINGPONG, vmath.vector4(0, 0, 1, 1), go.EASING_INOUTSINE, 5)
end

function magnetic_update(self, dt) 
	msg.post("/manager", "target_closest_ship", {pos = go.get_position(), range = 10000, dt = dt})
end

function magnetic_message_handling(self, message_id, message, sender) 
	if message_id == hash("target_ship_response") then
		if message.found then
			local vector_to_target = vmath.normalize(go.get_position(message.ship) - go.get_position())
			local dist = vmath.length(go.get_position(message.ship) - go.get_position())
			local force = vector_to_target * go.get("#co" .. self.size, "mass") / math.pow(dist, 1.75) * message.dt * 2000000000
			if vmath.length(force) > 500 then 
				force = vmath.normalize(force) * 500
			end
			msg.post("#co" .. self.size, "apply_force", {
				force = force, 
				position = go.get_position()
			})
		end
	end
end