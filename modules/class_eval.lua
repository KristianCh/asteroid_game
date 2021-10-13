require "modules.values.class_bonuses"
require "modules.values.store_texts"

function get_class_bonus(class, level)
	if class_bonuses[class].mix == "add" then
		return level * class_bonuses[class].value_per_level
	else
		return math.pow(class_bonuses[class].value_per_level, level)
	end
	return 0
end

function get_classes_text(classes)
	local text = ""
	for k, v in pairs(classes) do
		text = text .. class_texts[v][1] .. "\n"
	end
	return text
end

function get_active_class_bonus_text(class, level)
	local text = class_texts[class][2]
	class = hash(class)
	local value = string.format("%.2f", get_class_bonus(class, level))
	if class_bonuses[class].display == "percentage1-_dec" then
		text = text .. "<color=1,0.7,0.5,1>-" .. (1-value) * 100 .. "%</color>"
	elseif class_bonuses[class].display == "percentage-1_dec" then
		text = text .. "<color=1,0.7,0.5,1>-" .. (value-1) * 100 .. "%</color>"
	elseif class_bonuses[class].display == "percentage_dec" then
		text = text .. "<color=1,0.7,0.5,1>-" .. value * 100 .. "%</color>"
	elseif class_bonuses[class].display == "percentage1-_inc" then
		text = text .. "<color=1,0.7,0.5,1>+" .. (1-value) * 100 .. "%</color>"
	elseif class_bonuses[class].display == "percentage-1_inc" then
		text = text .. "<color=1,0.7,0.5,1>+" .. (value-1) * 100 .. "%</color>"
	elseif class_bonuses[class].display == "percentage_inc" then
		text = text .. "<color=1,0.7,0.5,1>+" .. value * 100 .. "%</color>"
	elseif class_bonuses[class].display == "direct_dec" then
		text = text .. "<color=1,0.7,0.5,1>-" .. value .. "</color>"
	elseif class_bonuses[class].display == "direct_inc" then
		text = text .. "<color=1,0.7,0.5,1>+" .. value .. "</color>"
	end
	return text
end



