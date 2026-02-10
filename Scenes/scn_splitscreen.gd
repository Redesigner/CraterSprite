extends Control

@export var camera0 : Camera2D
@export var camera1 : Camera2D

func _ready() -> void:
	camera1.get_viewport().world_2d = camera0.get_viewport().world_2d
