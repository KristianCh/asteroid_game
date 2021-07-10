components {
  id: "base_asteroid"
  component: "/game_objects/asteroids/base_asteroid.script"
  position {
    x: 0.0
    y: 0.0
    z: 0.0
  }
  rotation {
    x: 0.0
    y: 0.0
    z: 0.0
    w: 1.0
  }
}
embedded_components {
  id: "mesh"
  type: "mesh"
  data: "material: \"/render/materials/asteroid_material/asteroid_material.material\"\n"
  "vertices: \"/game_objects/asteroids/empty.buffer\"\n"
  "primitive_type: PRIMITIVE_TRIANGLES\n"
  "position_stream: \"position\"\n"
  ""
  position {
    x: 0.0
    y: 0.0
    z: 0.0
  }
  rotation {
    x: 0.0
    y: 0.0
    z: 0.0
    w: 1.0
  }
}
