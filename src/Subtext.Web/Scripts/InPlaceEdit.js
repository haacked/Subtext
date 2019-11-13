// InPlaceEdit.js
// Copyright (c) Nikhil Kothari, 2005
// http://www.nikhilk.net
//
// Feel free to use this sample, which is provided as-is.
// Please maintain the above comment if you decide to use this script.
//

Type.registerNamespace('nStuff.Samples.InPlaceEdit');

nStuff.Samples.InPlaceEdit.InPlaceEditBehavior = function() {
    nStuff.Samples.InPlaceEdit.InPlaceEditBehavior.initializeBase(this);
    
    var _labelCssClass;
    var _labelHoverCssClass;
    
    var _labelElement;
    var _isEditing = false;
    var _isInputControl = false;
    
    var _textBoxBlurHandler;
    var _labelFocusHandler;
    var _labelMouseOverHandler;
    var _labelMouseOutHandler;
    var _validatedHandler;
    
    this.get_isEditing = function() {
        return _isEditing;
    }
    
    this.get_labelCssClass = function() {
        return _labelCssClass;
    }
    this.set_labelCssClass = function(value) {
        _labelCssClass = value;
    }
    
    this.get_labelHoverCssClass = function() {
        return _labelHoverCssClass;
    }
    this.set_labelHoverCssClass = function(value) {
        _labelHoverCssClass = value;
    }
    
    this.beginEdit = function() {
        if (_isEditing) {
            return;
        }

        var textBoxElement = this.control.element;
        textBoxElement.style.display = '';
        _labelElement.style.display = 'none';

        textBoxElement.focus();

        _isEditing = true;
        this.raisePropertyChanged('isEditing');
    }
    
    this.dispose = function() {
        if (_labelElement) {
            _labelElement.detachEvent('onfocus', _labelFocusHandler);
            _labelElement.detachEvent('onmouseover', _labelMouseOverHandler);
            _labelElement.detachEvent('onmouseout', _labelMouseOutHandler);
            
            _labelElement = null;
            _labelFocusHandler = null;
            _labelMouseOverHandler = null;
            _labelMouseOutHandler = null;
        }

        if (_textBoxBlurHandler) {
            var textBoxElement = this.control.element;
            textBoxElement.detachEvent('onblur', _textBoxBlurHandler);
            _textBoxBlurHandler = null;
        }
        
        if (_validatedHandler) {
            this.control.validated.remove(_validatedHandler);
            _validatedHandler = null;
        }
        
        nStuff.Samples.InPlaceEdit.InPlaceEditBehavior.callBaseMethod(this, 'dispose');
    }
    
    this.endEdit = function() {
        if (!_isEditing) {
            return;
        }
        if (_isInputControl && this.control.get_isInvalid()) {
            return;
        }

        var textBoxElement = this.control.element;
        _labelElement.innerHTML = textBoxElement.value;
        _labelElement.style.display = 'block';
        textBoxElement.style.display = 'none';

        _isEditing = false;
        this.raisePropertyChanged('isEditing');
    }

    this.getDescriptor = function() {
        var td = nStuff.Samples.InPlaceEdit.InPlaceEditBehavior.callBaseMethod(this, 'getDescriptor');
        
        td.addProperty('isEditing', Boolean, /* readOnly */ true);
        td.addProperty('labelCssClass', String);
        td.addProperty('labelHoverCssClass', String);
        td.addMethod('beginEdit');
        td.addMethod('endEdit');
        return td;
    }
    
    this.initialize = function() {
        nStuff.Samples.InPlaceEdit.InPlaceEditBehavior.callBaseMethod(this, 'initialize');

        _labelElement = document.createElement('LABEL');
        
        var textBoxElement = this.control.element;
        var textBoxBounds = Web.UI.Control.getBounds(textBoxElement);
        var containerElement = document.createElement('SPAN');
        
        textBoxElement.parentNode.insertBefore(containerElement, textBoxElement);
        containerElement.appendChild(textBoxElement);
        containerElement.appendChild(_labelElement);

        textBoxElement.style.display = 'none';
        _labelElement.innerHTML = textBoxElement.value;
        _labelElement.tabIndex = textBoxElement.tabIndex;
        _labelElement.className = _labelCssClass;
        _labelElement.style.display = 'block';
        _labelElement.style.width = textBoxBounds.width + 'px';
        _labelElement.style.height = textBoxBounds.height + 'px';

        _textBoxBlurHandler = Function.createDelegate(this, this._onTextBoxBlur);
        _labelFocusHandler = Function.createDelegate(this, this._onLabelFocus);
        _labelMouseOverHandler = Function.createDelegate(this, this._onLabelMouseOver);
        _labelMouseOutHandler = Function.createDelegate(this, this._onLabelMouseOut);

        textBoxElement.attachEvent('onblur', _textBoxBlurHandler);
        if (Web.Application.get_type() == Web.ApplicationType.InternetExplorer) {
            _labelElement.attachEvent('onfocus', _labelFocusHandler);
        }
        else {
            _labelElement.attachEvent('onclick', _labelFocusHandler);
        }
        _labelElement.attachEvent('onmouseover', _labelMouseOverHandler);
        _labelElement.attachEvent('onmouseout', _labelMouseOutHandler);
        
        if (Web.UI.InputControl.isInstanceOfType(this.control)) {
            _isInputControl = true;
            _validatedHandler = Function.createDelegate(this, this._onValidated);
            this.control.validated.add(_validatedHandler);
        }
    }
    
    this._onLabelFocus = function() {
        if (_labelHoverCssClass && _labelHoverCssClass.length) {
            Web.UI.Control.removeCssClass(_labelElement, _labelHoverCssClass);
        }

        this.beginEdit();        
    }
    
    this._onLabelMouseOut = function() {
        if (_labelHoverCssClass && _labelHoverCssClass.length) {
            Web.UI.Control.removeCssClass(_labelElement, _labelHoverCssClass);
        }
    }
    
    this._onLabelMouseOver = function() {
        if (_labelHoverCssClass && _labelHoverCssClass.length) {
            Web.UI.Control.addCssClass(_labelElement, _labelHoverCssClass);
        }
    }
    
    this._onTextBoxBlur = function() {
        this.endEdit();
    }
    
    this._onValidated = function(sender, eventArgs) {
        if (this.control.get_isInvalid()) {
            this.beginEdit();
        }
    }
}
Type.registerSealedClass('nStuff.Samples.InPlaceEdit.InPlaceEditBehavior', Web.UI.Behavior);
Web.TypeDescriptor.addType('nk', 'inPlaceEdit', nStuff.Samples.InPlaceEdit.InPlaceEditBehavior);
