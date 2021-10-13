function table_size(table)
	local count = 0
	for _ in pairs(table) do count = count + 1 end
	return count
end

function table_contains(table, element)
	for k,v in pairs(table) do
		if v == element then
			return true
		end
	end
	return false
end

function set_table_default(table, val)
	local mt = {__index = function () return val end}
	setmetatable(table, mt)
end

function copy_table(table)
	local new = {}
	for k,v in pairs(table) do
		new[k] = v
	end
	return new
end

function sign(n) 
	if n >= 0 then return 1
	else return -1 end
end

function clamp(x, min, max)
	if x < min then return min end
	if x > max then return max end
	return x
end