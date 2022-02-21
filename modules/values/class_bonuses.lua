class_bonuses = {
	[hash("physical")] = 
	{
		affected_property = "armor", value_per_level = 1, class_masks = {"physical"}, mix = "add", 
		display = "direct_inc", target = "fleet"
	},
	[hash("energy")] = 
	{
		affected_property = "cooldown_mult", value_per_level = 0.95, class_masks = {"all"}, 
		mix = "multiply", display = "percentage1-_dec", target = "fleet"
	},
	[hash("guided")] = 
	{
		affected_property = "cooldown_mult", value_per_level = 0.9, class_masks = {"guided"}, 
		mix = "multiply", display = "percentage1-_dec", target = "fleet"
	},
	[hash("support")] = 
	{
		affected_property = "max_health", value_per_level = 5, class_masks = {"physical"}, 
		mix = "add", display = "direct_inc", target = "fleet"
	},
	[hash("graviton")] = 
	{
		affected_property = "physical_damage_mult", value_per_level = 0.1, class_masks = {"asteroid"}, 
		mix = "multiply", display = "percentage_inc", target = "asteroid"
	}
}