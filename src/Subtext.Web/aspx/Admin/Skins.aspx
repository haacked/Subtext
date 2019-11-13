<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Skins.aspx.cs" Inherits="Subtext.Web.Admin.Skins" %>
<asp:Content ID="Content1" ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Actions</h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageContent" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js"></script>
    <script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/aspx/Admin/js/jquery.tmpl.min.js") %>"></script>
    <script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/aspx/Admin/js/knockout-1.2.1.js") %>"></script>
    <script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/aspx/Admin/js/jquery-impromptu.3.1.min.js") %>"></script>

    <script type="text/javascript">
        $(function() {
            $.ajaxSetup ({cache: false});

            var selectedSkin = $('#skin-container').data('selected-skin');
            var selectedMobileSkin = $('#skin-container').data('selected-mobile-skin');
            var installedPackages = <%= Json(InstalledPackages) %>;
            var mobilePackages = <%= Json(MobilePackages) %>;

            window.viewModel = {
                urls: {
                    install: '<%: Url.SkinController("install") %>',
                    uninstall: '<%: Url.SkinController("uninstall") %>',
                    online: '<%: Url.SkinController("online") %>',
                    updates: '<%: Url.SkinController("updates") %>',
                    save: '<%: Url.SkinController("save") %>'
                },

                installed: ko.observableArray(installedPackages),
                
                mobile: ko.observableArray(mobilePackages),
                
                online: ko.observableArray([]),
                
                onlineNotInstalled: ko.observableArray([]),

                getOnlinePackages: function() {
                    var $this = this;
                    return $.when($.getJSON(this.urls.online))
                            .done(
                                function(data) {
                                    $.each(data, function() {
                                        this.loading = ko.observable(false);
                                    });
                                    $this.online(data);
                                }
                            );
                },

                checkingForUpdates: ko.observable(false),

                checkForUpdates: function() {
                    this.checkingForUpdates(true);
                    var $this = this;
                    $.when($.getJSON(this.urls.updates))
                    .fail(function() {
                        $.prompt('Checking for updates failed');
                        $this.checkingForUpdates(false);
                    })
                    .done(function(data){
                        var updates = data;
                        // Build a hash of updates
                        var updatedPackages = [];
                        $.each(updates, function() { updatedPackages[this.id] = this });

                        // Update each package with the update
                        $.each($this.installed(), function() {
                            var availableUpdate = updatedPackages[this.id];
                            if (availableUpdate) {
                                this.hasUpdate(true);
                                this.availableUpdate = availableUpdate;
                            }
                        });

                        $this.checkingForUpdates(false);
                    });
                },

                uninstall: function(package) {
                    var $this = this;
                    prompt("Uninstall " + package.Id + "?", "This will remove the following skins: " + getSkinList(package), function() {
                        package.loading(true);
                        $.when($.post($this.urls.uninstall, {id: package.id}))
                        .done(function(data) {
                            if (!data.success) {
                                $.prompt(data.message);
                            }
                            $this.installed.remove(package);
                        })
                        .fail(function(data) {
                            $.prompt('<h2>Uninstall failed!</h2>');
                            package.loading(false);
                        });
                    });
                },
                
                update: function(package) {
                    var $this = this;
                    prompt("Update " + package.Id + " to version " + package.availableUpdate.version + "?", "This will update the following skins: " + getSkinList(package), function() {
                        $.when($.post($this.urls.uninstall, {id: package.id}))
                        .done(function(data) {
                            if (!data.success) {
                                package.version(data.version);
                                package.hasUpdate(false);
                            }
                            $this.installed.remove(package);
                        })
                        .fail(function(data) {
                            $.prompt('<h2>Uninstall failed!</h2>');
                        });
                    });
                },
                
                install: function(package) {
                    var $this = this;
                    prompt("Install " + package.id + " " + package.version + "?", "", function() {
                        package.loading(true);
                        $.when($.post($this.urls.install, {id: package.id}))
                        .done(function(data) {
                            $this.initPackage(data);
                            $this.installed.push(data);
                        })
                        .fail(function(data) {
                            $.prompt('<h2>Install failed!</h2>');
                            package.loading(false);
                        });
                    });
                },
                
                selectSkin: function(skin) {
                    if (!skin.mobile && skin.name === this.selectedSkin().name) {
                        return;
                    }
                    if (skin.mobile && skin.name === this.selectedMobileSkin().name) {
                        return;
                    }

                    var $this = this;
                    $.when($.post(this.urls.save, {skinKey: skin.skinKey, mobile: skin.mobile }))
                    .done(
                        function() {
                            var element;
                            if (!skin.mobile) {
                                $this.selectedSkin(skin);
                                element = $('div.skin.selected div.notice');
                            }
                            else {
                                $this.selectedMobileSkin(skin);
                                element = $('div.skin.selectedMobile div.notice')
                            }
                            element.css('color', '#eecc00').fadeIn()
                            .animate({ opacity: 1.0, color: '#006600' }, 400)
                            .delay(1000)
                            .fadeOut(1000);
                        }
                    ).fail(function() {
                        $.prompt('Setting the skin failed.');
                    });
                },
                // Should be set from data in the page.
                selectedSkin: ko.observable(selectedSkin),
                
                selectedMobileSkin: ko.observable(selectedMobileSkin),

                initPackage: function(package) {
                    package.hasUpdate = ko.observable(false);
                    package.version = ko.observable(package.version);
                    package.loading = ko.observable(false);
                },

                init: function() {
                    var $this = this;
                    $.each(this.installed(), function() {
                        $this.initPackage(this);
                    });

                    $.each(this.mobile(), function() {
                        $this.initPackage(this);
                    });

                    this.onlineNotInstalled = ko.dependentObservable(function() {
                        // Build a hash of installed IDs
                        var installedIds = {};
                        $.each(this.installed(), function() { 
                            installedIds[this.id] = true; 
                        });
                    
                        // Filter out online packages whose ID is in the hash
                        return $.grep(this.online(), function(item) { 
                                return !installedIds[item.id];
                        });
                    }
                    , this);

                    var $this = this;
                    this.getOnlinePackages().done(function(){$this.checkForUpdates()});
                }
            };

            window.viewModel.init();

            var getSkinList = function(package) {
                var list = "<ul>";
                for (var i = 0; i < package.skins.length; i++) {
                    list += "<li>" + package.skins[i].name + "</li>";
                }
                return list += "</ul>";
            };

            var prompt = function(title, message, callback) {
                var text = "<h2>" + title + "</h2>" +
                "<p>" + message + "</p>";

                $.prompt(text, {
                    callback: function(ok) {
                        if (ok) {
                            callback();
                        }
                    },
                    buttons: { Ok: true, Cancel: false }
                });
            }

            ko.applyBindings(window.viewModel);
        });
    </script>

    <script type="text/html" id="packageTemplate">
        <div class="package">
            <div data-bind="css: {loading: $data.loading() === true}"></div>
            <div class="package-header">
                <label>Skin Package: ${id} Version: ${version}</label>
                <div class="actions">
                    <a href="#" data-bind="click: function(){window.viewModel.update($data)}, 
                        visible: hasUpdate,
                        afterAdd: function(elem){ $(elem).hide().fadeIn() }">update</a> 
                    <a href="#" data-bind="click: function(){window.viewModel.uninstall($data);}">uninstall</a>
                </div>
            </div>
            <div data-bind='template: { name: "skinTemplate", foreach: skins }'></div>
        </div>
    </script>

    <script type="text/html" id="skinTemplate">
        <div class="skin" data-bind="
                click: function(){ window.viewModel.selectSkin($data)}, 
                css: {
                    selected: window.viewModel.selectedSkin().name === $data.name,
                    selectedMobile: window.viewModel.selectedMobileSkin().name === $data.name,
                },
                style: {
                    cursor: (window.viewModel.selectedMobileSkin().name === $data.name && $data.mobile) || (window.viewModel.selectedSkin().name === $data.name && !$data.mobile) ? 'default' : 'pointer'
                }">
            <div class="notice">
                Skin Saved
            </div>
            <img src="${icon}" class="skin-thumbnail" alt="Skin" />
            <label>${name}</label>
        </div>
    </script>

    <script type="text/html" id="onlinePackageTemplate">
        <tr>
            <td>${id}</td>
            <td>${version}</td> 
            <td style="position: relative">
                <div data-bind="css: {loading: $data.loading() === true}"></div>
                <a href="#" data-bind="click: function() {window.viewModel.install($data)}, visible: $data.loading() === false">Install</a>
            </td>
        </tr>
    </script>


    <div id="skin-container" data-selected-skin='<%= Json(SelectedSkin) %>' data-selected-mobile-skin='<%= Json(SelectedMobileSkin) %>'>
        <div class="current-skin">
            <h3>Current Skin</h3>
            <div data-bind='template: {
                name: "skinTemplate", 
                data: window.viewModel.selectedSkin(),
                afterRender: function(elem){ $(elem).hide().fadeIn() }
            }'></div>
        </div>

        <div id="mobile-skin" class="current-skin">
            <h3>Current Mobile Skin</h3>
            <div data-bind='template: {
                name: "skinTemplate", 
                data: window.viewModel.selectedMobileSkin(),
                afterRender: function(elem){ $(elem).hide().fadeIn() }
            }'></div>
        </div>

        <h2>Choose A Skin</h2>
        <button id="check-updates" style="width: 180px;" data-bind="visible: !checkingForUpdates(), click: checkForUpdates">Check for updates     </button>
        <div style="position: relative; width: 600px;" data-bind="visible: checkingForUpdates()">
            <button disabled="disabled" style="display: inline:block; width: 180px;">Checking For Updates...</button>
            <div class="loading" style="display: inline-block; width: 180px;"></div>
        </div>

        <div class="installedSkins non-mobile" data-bind='template: { 
                name: "packageTemplate", 
                foreach: installed, 
                beforeRemove: function(elem){ $(elem).fadeOut() },
                afterAdd: function(elem){ $(elem).hide().fadeIn() } 
        }'>
        </div>

        <h2>Mobile</h2>
        <div class="installedSkins mobile" data-bind='template: { 
                name: "packageTemplate", 
                foreach: mobile, 
                beforeRemove: function(elem){ $(elem).fadeOut() },
                afterAdd: function(elem){ $(elem).hide().fadeIn() } 
        }'>
        </div>

        <h2>Available</h2>
        <table class="listing">
            <thead>
            <tr>
                <th>Skin Package</th>
                <th>Description</th>
                <th>Action</th>
            </tr>
            </thead>
            <tbody data-bind='template: { 
                name: "onlinePackageTemplate", 
                foreach: onlineNotInstalled,
                beforeRemove: function(elem){ $(elem).slideUp() },
                afterAdd: function(elem){ $(elem).hide().fadeIn() } 
            }'>
            </tbody>
        </table>
    </div>
</asp:Content>