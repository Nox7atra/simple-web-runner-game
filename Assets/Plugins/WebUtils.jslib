var LibraryGLClear = {
	OnGameOver: function(){
		window.dispatchReactUnityEvent(
		  "OnGameOver"
		);
	},
	OnScoreUpdate: function(scoreValue){
		window.dispatchReactUnityEvent(
			"OnScoreUpdate",
			scoreValue
		);
	},
	OnFloatingText: function(x, y, text){
		window.dispatchReactUnityEvent(
			"OnFloatingText",
			x,
			y,
			UTF8ToString(text)
		);
	}
};
mergeInto(LibraryManager.library, LibraryGLClear); 
