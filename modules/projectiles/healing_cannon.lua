function healing_cannon_init(self)
	self.spawn_chance = self.level * 0.1
	self.value = 5 + self.level * 5
end

function healing_cannon_on_message(self, message_id, message, sender)
	if message_id == hash("trigger_response") and message.enter then
		msg.post(message.other_id, "damage_asteroid", {damage = self.damage, type = "physical"})
		go.delete()
	elseif message_id == hash("damage_was_fatal") and math.random() < self.spawn_chance then
		factory.create("/manager#player_mine_factory", go.get_position(), nil, {
			heading = self.heading, value = self.value, 
			angular_velocities = vmath.vector3(self.heading.y, self.heading.x, 0) * 2,
			type = 2
		}, vmath.vector3(0.5))
	end
end