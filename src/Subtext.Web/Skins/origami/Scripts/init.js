
function init(){
	bodyEl = document.getElementsByTagName('body')[0];
	/* register rules */
	//Behaviour.register(searchRules);
	Behaviour.register(styleSwitcherRules);
}

var isCommentFormToggled = false;
function toggleCommentForm(){
	if (!isCommentFormToggled){
		Effect.Appear('guest_email');
		Effect.Appear('guest_url');
		isCommentFormToggled = true;
	} else {
		Effect.Fade('guest_email');
		Effect.Fade('guest_url');
		isCommentFormToggled = false;
	}
}