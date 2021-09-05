function table_size(table)
	local count = 0
	for _ in pairs(table) do count = count + 1 end
	return count
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