function missile_init(self)
	msg.post(self.target, "subscribe")
end

function missile_update(self, dt)	
	local pos = go.get_position()

	local vec_to_target = self.heading
	if self.target ~= nil then 
		vec_to_target = vmath.normalize(go.get_position(self.target) - pos)
	end

	local d_steer = vmath.cross(vmath.slerp(self.aim * dt, self.heading, vec_to_target), self.heading).z * 10000
	self.d_steer = vmath.lerp(self.aim * dt, self.d_steer, d_steer)
	self.heading = vmath.normalize(vmath.slerp(self.aim * dt, self.heading, vec_to_target))
	local angle = math.deg(math.atan2(self.heading.y, self.heading.x)) - 90
	local rot_z = vmath.quat_rotation_z(math.rad(angle))
	local rot_y = vmath.quat_rotation_y(math.rad(self.d_steer))
	go.set_rotation(rot_z * rot_y)

	pos = pos + vmath.normalize(self.heading) * self.speed * dt
	go.set_position(pos)

	self.rot = self.rot + dt * 500
	local rot_z = vmath.quat_rotation_y(math.rad(self.rot))
	local current_rot = go.get_rotation()
	go.set_rotation(current_rot * rot_z)
end

function missile_on_message(self, message_id, message, sender)
	if message_id == hash("trigger_response") and message.enter then
		msg.post(message.other_id, "damage_asteroid", {damage = self.damage})
		go.delete()
	elseif message_id == hash("unsubscribe") then
		self.target = nil
	end
end