function cannon_init(self)
	--msg.post("#sprite", "play_animation", {id = hash("cannon_round")})
end

function cannon_update(self, dt)
	local pos = go.get_position() + vmath.normalize(self.heading) * self.speed * dt
	go.set_position(pos)

	self.rot = self.rot + dt * 2500
	local rot_z = vmath.quat_rotation_y(math.rad(self.rot * dt))
	local current_rot = go.get_rotation()
	go.set_rotation(current_rot * rot_z)
end

function cannon_on_message(self, message_id, message, sender)
	if message_id == hash("trigger_response") and message.enter then
		msg.post(message.other_id, "damage_asteroid", {damage = self.damage, type = "physical"})
		go.delete()
	end
end