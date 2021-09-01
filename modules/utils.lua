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