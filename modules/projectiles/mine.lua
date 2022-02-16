function mine_init(missile)
	--msg.post("#sprite", "play_animation", {id = hash("cannon_round")})
end

function mine_update(self, dt)
	local pos = go.get_position() + self.heading * self.speed * dt
	go.set_position(pos)

	local euler = go.get(".", "euler")
	go.set(".", "euler", euler + self.angular_velocities)
	
	self.heading = self.heading * (1 - dt)
	self.angular_velocities = self.angular_velocities * (1 - dt)
end

function mine_on_message(self, message_id, message, sender)
	if message_id == hash("collision_response") and message.other_group == hash("asteroid") then
		if message.own_group == hash("player_mine_range") then
			local vector_to_other = message.other_position - go.get_position()
			local force_velocity = vmath.normalize(vector_to_other) * self.acceleration * (1 / vmath.length(vector_to_other))
			self.heading = self.heading + force_velocity

			self.angular_velocities = self.angular_velocities + vmath.vector3(force_velocity.y, force_velocity.x, 0)
		else
			msg.post(message.other_id, "damage_asteroid", {damage = self.value, type = "physical"})
			go.delete()
		end
	end
end