function healing_mine_on_message(self, message_id, message, sender)
	if message_id == hash("collision_response") and message.other_group == hash("ship") then
		if message.own_group == hash("player_mine_range") then
			local vector_to_other = message.other_position - go.get_position()
			local force_velocity = vmath.normalize(vector_to_other) * self.acceleration * (1 / vmath.length(vector_to_other))
			self.heading = self.heading + force_velocity

			self.angular_velocities = self.angular_velocities + vmath.vector3(force_velocity.y, force_velocity.x, 0)
		else
			msg.post(message.other_id, "heal", {value = self.value})
			go.delete()
		end
	end
end