/*#############################################################
Name: Niceforms
Version: 0.9
Author: Lucian Slatineanu
URL: http://www.badboy.ro/

Feel free to use and modify but please provide credits.
#############################################################*/

//global variables that can be used by all the functions on this page.
var selects;
var inputs;
var radios = new Array();
var checkboxes = new Array();
var hovers = new Array();
var buttons = new Array();
var selectText = "please select";

//this function runs when the page is loaded so put all your other onload stuff in here too.
function init() {
	
	//check if styles are enabled and only then start replacing elements
	if(findPosX(document.getElementById('stylesheetTest')) == -999) {
		replaceSelects();
		replaceRadios();
		replaceCheckboxes();
	}
	hoverEffects();
	buttonHovers();
}

function replaceRadios() {
	//get all the radio buttons on the page
	var inputs = document.getElementsByTagName('input');
	var j = 0;
	for(var i=0; i < inputs.length; i++) {
		if(inputs[i].type=='radio') {
			radios[j] = inputs[i];
			++j;
		}
	}
	
	//cycle through the radio inputs
	for(var i=0; i <radios.length; i++) {
		
		//make them transparent
		radios[i].className = "transparent";
		
		//get their position
		var x = findPosX(radios[i]);
		var y = findPosY(radios[i]);
		
		//build new div
		var radioArea = document.createElement('div');
		if(radios[i].checked) {radios[i].nextSibling.className = "chosen"; radioArea.className = "radioAreaChecked";}
		else if(!radios[i].checked) {radioArea.className = "radioAreaUnchecked";}
		radioArea.style.left = x + 'px';
		radioArea.style.top = y + 'px';
		radioArea.id = 'myRadio'+i;
		radios[i].onclick = new Function('checkRadio('+i+')');
		
		//insert div
		document.getElementsByTagName("body")[0].appendChild(radioArea);
	}
}

function replaceCheckboxes() {
	//get all the checkboxes on the page
	var inputs = document.getElementsByTagName('input');
	var j = 0;
	for (var i2=0; i2 < inputs.length; i2++) {
		if(inputs[i2].type=='checkbox') {
			checkboxes[j] = inputs[i2];
			++j;
		}
	}

	//cycle through the checkboxes
	for(var i2=0; i2 < checkboxes.length; i2++) {

		//make them transparent
		checkboxes[i2].className = "transparent";

		//get their position
		var x = findPosX(checkboxes[i2]);
		var y = findPosY(checkboxes[i2]);

		//build new div
		var checkboxArea = document.createElement('div');
		if(checkboxes[i2].checked) {checkboxes[i2].nextSibling.className = "chosen"; checkboxArea.className = "checkboxAreaChecked";}
		else if(!checkboxes[i2].checked) {checkboxArea.className = "checkboxAreaUnchecked";}
		checkboxArea.style.left = x + 'px';
		checkboxArea.style.top = y + 'px';
		checkboxArea.id = 'myCheck'+i2;
		checkboxes[i2].onclick = new Function('checkCheck('+i2+')');

		//insert div
		document.getElementsByTagName("body")[0].appendChild(checkboxArea);
	}
}

function replaceSelects() {
	//get all the select fields on the page
    selects = document.getElementsByTagName('select');
	
	//cycle trough the select fields
    for(var i=0; i < selects.length; i++) {
		
		//create and build div structure
		var selectArea = document.createElement('div');
		var left = document.createElement('div');
		var right = document.createElement('div');
		var center = document.createElement('div');
		var button = document.createElement('a');
		var text = document.createTextNode(selectText);
		center.id = "mySelectText"+i;
		button.href="javascript:showOptions("+i+")";
		selectArea.className = "selectArea";
		left.className = "left";
		right.className = "right";
		center.className = "center";
		right.appendChild(button);
		center.appendChild(text);
		selectArea.appendChild(left);
		selectArea.appendChild(right);
		selectArea.appendChild(center);
		
		//hide the select field
        selects[i].style.display='none'; 
		
		//insert select div
		selects[i].parentNode.insertBefore(selectArea, selects[i]);
		
		//build & place options div
		var optionsDiv = document.createElement('div');
		optionsDiv.className = "optionsDivInvisible";
		optionsDiv.id = "optionsDiv"+i;
		optionsDiv.style.left = findPosX(selectArea) + 'px';
		optionsDiv.style.top = findPosY(selectArea) + 19 + 'px';
		
		//get select's options and add to options div
		for(var j=0; j < selects[i].options.length; j++) {
			var optionHolder = document.createElement('p');
			var optionLink = document.createElement('a');
			var optionTxt = document.createTextNode(selects[i].options[j].text);
			optionLink.href = "javascript:showOptions("+i+"); selectMe('"+selects[i].id+"',"+j+","+i+");";
			optionLink.appendChild(optionTxt);
			optionHolder.appendChild(optionLink);
			optionsDiv.appendChild(optionHolder);
		}
		
		//insert options div
		document.getElementsByTagName("body")[0].appendChild(optionsDiv);
	}
}

function showOptions(g) {
		elem = document.getElementById("optionsDiv"+g);
		if(elem.className=="optionsDivInvisible") {elem.className = "optionsDivVisible";}
		else if(elem.className=="optionsDivVisible") {elem.className = "optionsDivInvisible";}
}

function selectMe(selectFieldId,linkNo,selectNo) {
	//feed selected option to the actual select field
	selectField = document.getElementById(selectFieldId);
	for(var k = 0; k < selectField.options.length; k++) {
		if(k==linkNo) {
			selectField.options[k].selected = "selected";
		}
		else {
			selectField.options[k].selected = "";
		}
	}
	//show selected option
	textVar = document.getElementById("mySelectText"+selectNo);
	var newText = document.createTextNode(selectField.options[linkNo].value);
	textVar.replaceChild(newText, textVar.childNodes[0]);
}

function findPosY(obj) {
	var posTop = 0;
	while (obj.offsetParent) {
		posTop += obj.offsetTop;
		obj = obj.offsetParent;
	}
	return posTop;
}
function findPosX(obj) {
	var posLeft = 0;
	while (obj.offsetParent) {
		posLeft += obj.offsetLeft;
		obj = obj.offsetParent;
	}
	return posLeft;
}

function checkRadio(g) {
	if(radios[g].checked) {
		for (var k = 0; k < radios.length; k++)
		{
			if(k != g) {
				document.getElementById('myRadio'+k).className = "radioAreaUnchecked";
				radios[k].nextSibling.className = "";
			}
			else if(k == g) {
				document.getElementById('myRadio'+k).className = "radioAreaChecked";
				radios[g].nextSibling.className = "chosen";
			}
		}
	}
	else if(!radios[g].checked) {document.getElementById('myRadio'+g).className = "radioAreaUnchecked"; radios[g].nextSibling.className = "";}
}

function checkCheck(g) {
	if(checkboxes[g].checked) {
		for(var k = 0; k < checkboxes.length; k++) {
			if(k == g) {
				document.getElementById('myCheck'+k).className = "checkboxAreaChecked";
				checkboxes[g].nextSibling.className = "chosen";
			}
		}
	}
	else if(!checkboxes[g].checked) {
		document.getElementById('myCheck'+g).className = "checkboxAreaUnchecked";
		checkboxes[g].nextSibling.className = "";
	}
}

function hoverEffects() {
	//get all elements (text inputs, passwords inputs, textareas)
	var elements = document.getElementsByTagName('input');
	var j = 0;
	for (var i4 = 0; i4 < elements.length; i4++) {
		if((elements[i4].type=='text')||(elements[i4].type=='password')) {
			hovers[j] = elements[i4];
			++j;
		}
	}
	elements = document.getElementsByTagName('textarea');
	for (var i4 = 0; i4 < elements.length; i4++) {
		hovers[j] = elements[i4];
		++j;
	}
	
	//add focus effects
	for (var i4 = 0; i4 < hovers.length; i4++) {
		hovers[i4].onfocus = function() {this.className += "Hovered";}
		hovers[i4].onblur = function() {this.className = this.className.replace(/Hovered/g, "");}
	}
}

function buttonHovers() {
	//get all buttons
	var elements = document.getElementsByTagName('input');
	var j = 0;
	for (var i5 = 0; i5 < elements.length; i5++) {
		if(elements[i5].type=='submit') {
			buttons[j] = elements[i5];
			++j;
		}
	}
	
	//add hover effects
	for (var i5 = 0; i5 < buttons.length; i5++) {
		buttons[i5].onmouseover = function() {this.className += "Hovered";}
		buttons[i5].onmouseout = function() {this.className = this.className.replace(/Hovered/g, "");}
	}
}


window.onload = init;