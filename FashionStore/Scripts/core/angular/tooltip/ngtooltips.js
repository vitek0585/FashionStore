// =====================================================================
// ngTooltips Directive
//
// By Walter Staeblein - 2015
// =====================================================================
(function() {

    var app = angular.module('ngTooltips', []);

    app.directive('tooltip', function() {
        return {
            restrict: 'A',
            compile: function(element, attributes) {
                var tag = $('<div style="display: none"></div>');
                $('body').append(tag);

                return function(scope, element, attr) {

                    var triggerEvent = 'mouseover';
                    if (attr.tooltipTrigger && attr.tooltipTrigger == 'click') { triggerEvent = 'click'; }

                    $(element).on(triggerEvent, function(evt) { 

                        evt.stopPropagation();

                        var elePos = $(element).offset();
                        var content = attr.tooltip;
                        var top = 0, left = 0, xclass = attr.tooltipPos;

                        if (content.substring(0, 1) == '#') { content = $(content).html(); }
                        tag.html(content);
                        tag.removeClass().addClass('tooltip_box ' + xclass);

                        switch(xclass) {
                            case 'bottom':
                                top = elePos.top + tag.height() + 12;
                                left = elePos.left - 25;
                                break;
                            case 'right':
                                top = elePos.top - 5;
                                left = elePos.left + $(element).width() + 15;
                                break;
                            case 'left':
                                top = elePos.top - 5;
                                left = elePos.left - tag.width() - 30;
                                break;
                            default:
                                top = elePos.top - tag.outerHeight()-10;
                                left = (elePos.left + $(element).outerWidth() / 2) - tag.outerWidth() / 2;
                                xclass = 'top';
                                break;
                        }
                        if (top < 0) { top = 0; }
                        if (left < 0) { left = 0; }

                        tag.fadeIn(300).css({ 'top': top, 'left': left });

                        return false;
                    });
                    
                    if (attr.tooltipDismiss && attr.tooltipDismiss == 'click') {
                        tag.on('click', function() { tag.fadeOut(300); });
                    } else {
                        $(element).on('mouseleave', function(evt) { 

                            tag.fadeOut(300); 

                        });
                    }
                }
            }
        }
    });

} ());
