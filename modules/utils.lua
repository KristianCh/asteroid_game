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

function transform_mouse_input_coords_to_world_coords(mouse_input_coords) 
	x, y, w, h = defos.get_window_size()
	local def_ratio = 1920/1017
	local acc_ratio = w/h
	local x_scale = acc_ratio / def_ratio
	local y_scale = def_ratio / acc_ratio
	out = vmath.vector3(mouse_input_coords.x, mouse_input_coords.y, mouse_input_coords.z)
	if w >= h*def_ratio then
		local dif = (1920 * x_scale) - 1920
		local x_mapped = mouse_input_coords.x * x_scale
		x_mapped = x_mapped - dif/1.95
		out.x = x_mapped
		
		--mouse_input_coords.x = mouse_input_coords.x * w/h
	else
		local dif = (1017 * y_scale) - 1017
		local y_mapped = mouse_input_coords.y * y_scale
		y_mapped = y_mapped - dif/2
		out.y = y_mapped
	end
	return out
end