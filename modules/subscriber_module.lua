function init_subscriber_module(self)
	self.subscribers = {}
end

function notify_subscribers_of_destruction(self)
	for i,v in ipairs(self.subscribers) do 
		msg.post(v, "unsubscribe")
	end
end

function on_subscription_message(self, message_id, message, sender)
	if message_id == hash("subscribe") then
		self.subscribers[#self.subscribers+1] = sender
	elseif message_id == hash("unsubscribe") then
		for k,v in pairs(self.subscribers)do
			if v == sender then
				table.remove(self.subscribers, k)
				break
			end
		end
	end
end