class_bonuses = {
	[hash("physical")] = {affected_property = "armor", value_per_level = 1, class_masks = {"physical"}, mix = "additive"},
	[hash("energy")] = {affected_property = "cooldown_mult", value_per_level = 0.95, class_masks = {"all"}, mix = "multiply"},
	[hash("guided")] = {affected_property = "cooldown_mult", value_per_level = 0.9, class_masks = {"guided"}, mix = "multiply"}
}