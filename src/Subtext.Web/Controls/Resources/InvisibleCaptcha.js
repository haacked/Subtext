function subtext_invisible_captcha_hideFromJavascriptEnabledBrowsers(id)
{
	var captcha = document.getElementById(id);
	if(captcha)
	{
		captcha.style.display = 'none';
	}
}

function subtext_invisible_captcha_setAnswer(first, second, formInputId)
{
	var formInput = document.getElementById(formInputId);
	if(formInput)
	{
		formInput.value = (first + second);
	}
}