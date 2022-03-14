require "modules.utils"

function attack_drone_init(self)
	self.mothership = nil

	self.update_type = attack_drone_update
	self.message_type = attack_drone_message
end

function attack_drone_update(self, dt)
	local pos = go.get_position()
	local vec_to_target = vec_to_target = vmath.normalize(self.target_position - pos)
	self.heading = vmath.normalize(vmath.slerp(self.evasion * dt, self.heading, vec_to_target))
end

function attack_drone_message(self, message_id, message, sender)
	if message_id == hash("recieve_target") then
		if message.found then
			self.target_position = go.get_position(message.target)
		else
			self.target_position = go.get_position(self.mothership_url)
		end
	end
end