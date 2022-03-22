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
		mix = "add", display = "percentage_inc", target = "manager"
	},
	[hash("nuker")] = 
	{
		affected_property = "aoe_range_mult", value_per_level = 0.05, class_masks = {"aoe"}, 
		mix = "add", display = "percentage_inc", target = "manager"
	},
	[hash("carrier")] = 
	{
		affected_property = "drone_range", value_per_level = 1.1, class_masks = {"carrier"}, 
		mix = "multiply", display = "percentage-1_inc", target = "fleet"
	}
}