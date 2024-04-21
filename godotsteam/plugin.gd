tool
extends EditorPlugin

func _enter_tree():
	add_autoload_singleton("Steam", "res://godotsteam/steam.gd")

func _exit_tree():
	remove_autoload_singleton("Steam")
