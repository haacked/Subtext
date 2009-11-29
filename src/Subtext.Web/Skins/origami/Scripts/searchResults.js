var resultsAreShown = false;
var loaded = false;
var intervalID = 0;
var animation = false;
var isFocused = false;
var searchRules = {
	'#search-results-close' : function(el){
		el.onclick = function(){
			if($('q').value.length == 0){
				swapClass(el, 'focus', 'blur');
				loaded = false;
			}
			Effect.Fade('search-results-close');
			slideSearchUp();
		}
	},
	'#q' : function(el){
		el.onfocus = function(){
			isFocused = true;
			if(checkClass(el, 'blur')){
				swapClass(el, 'focus', 'blur');
			} else {
				addClass(el, 'focus');
			}
			intervalID  = setInterval(
				function(){
					if(el.value.length > 0){
						if(!checkClass(el, 'focus')){
							swapClass(el, 'focus', 'blur');
						}
						if(!resultsAreShown && !animation && loaded){
							slideSearchDown();
						}
					}
				} , 100
			)
		
		}
		el.onblur = function(){
			window.clearInterval(intervalID);
			Element.hide('search_spinner');
			if(el.value.length == 0){
				swapClass(el, 'focus', 'blur');
				loaded = false;
			}
			isFocused = false;
			setTimeout( slideSearchUp , 10000 )
		}
	}
};
/* rule registration can be fount in inti.js */

function slideSearchUp(){
	if(resultsAreShown && !isFocused){
		Effect.BlindUp('search-results',
			{
				beforeStart: function(){
					animation = true;
					Effect.Fade('search-results-close');
				},
				afterFinish: function(){
					animation = false;
					resultsAreShown = false;
				}
			} 
		)
	}
}

function slideSearchDown(){
	Effect.BlindDown('search-results',
		{
			beforeStart: function(){
				animation = true;
			}, 
			afterFinish: function(){
				animation = false;
				resultsAreShown = true;
				Effect.Appear('search-results-close');
			}
		}
	);	
}

function showResults(){
	Element.hide('search_spinner');
	loaded = true;
}