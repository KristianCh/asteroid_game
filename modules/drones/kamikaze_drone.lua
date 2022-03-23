require "modules.utils"

function kamikaze_drone_init(self)

	self.update_type = kamikaze_drone_update
	self.message_type = kamikaze_drone_message

	self.speed = 450
	
	self.mothership_type = "kamikaze_carrier"
	self.raycast_targets = { hash("ship_cast"), hash("player_drone") }
	go.set("#model", "tint", vmath.vector4(1, 0, 0, 1))
end

function kamikaze_drone_update(self, dt)
	msg.post("/manager", "get_special_ship_url", {type = self.mothership_type})
	
	local pos = go.get_position()
	local vec_to_target = vmath.normalize(self.target_position - pos)
	self.heading = vmath.normalize(vmath.slerp(self.evasion * dt, self.heading, vec_to_target))
end

function kamikaze_drone_message(self, message_id, message, sender)
	if message_id == hash("return_special_ship_url") then
		if message.url ~= nil then
			msg.post(message.url, "request_target")
		else
			msg.post(".", "self_destruct")
		end
	elseif message_id == hash("recieve_target") then
		self.target_position = message.target_position
	elseif message_id == hash("contact_point_response") then
		if message.other_group == hash("asteroid") then
			msg.post( "/manager", "create_player_aoe", 
			{
				position = go.get_position(), 
				rotation = vmath.quat_rotation_z(math.random(-math.pi, math.pi)),
				properties = {duration = 0.5, damage = self.damage, type = 3}, 
				scale = vmath.vector3(self.aoe_range)
			})
		end
	end
end