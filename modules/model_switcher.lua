SHIP = 1
PLAYER_PROJECTILE = 2
PLAYER_MINE = 3

SHIP_NUMBER = 11
PLAYER_PROJECTILE_NUMBER = 3
PLAYER_MINE_NUMBER = 2

ship_modelnames = {
	"#battleship", "#laser_cutter", "#missile_cruiser", "#minelayer", "#mine_healer", 
	"#gravity_tether", "#graviton_trapper", "#energy_nuker", "#ram", "#fleet_carrier",
	"#kamikaze_carrier"
}
player_projectile_modelnames = {"#cannon_round", "#missile", "#healing_cannon_round"}
player_mine_modelnames = {"#mine", "#healing_mine"}

function model_switch(model_number, model_set)
	total_number = SHIP_NUMBER
	modelnames = ship_modelnames
	if model_set == SHIP then
		total_number = SHIP_NUMBER
		modelnames = ship_modelnames
	elseif model_set == PLAYER_PROJECTILE then
		total_number = PLAYER_PROJECTILE_NUMBER
		modelnames = player_projectile_modelnames
	elseif model_set == PLAYER_MINE then
		total_number = PLAYER_MINE_NUMBER
		modelnames = player_mine_modelnames
	else 
		return
	end 
	
	for i=1, total_number do
		if i ~= model_number then
			msg.post(modelnames[i], "disable")
		end
	end
end