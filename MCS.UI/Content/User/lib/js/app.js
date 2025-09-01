var WindowWidth = $(window).width(), Windowheight = $(window).height();//used to fix chrome mob known issue
var DeliveryMethodType = { Paper: 230, Electronic: 231, ElectronicPaper: 232 };
var TransactionType = { None: 1295, Inbound: 14, ExternalOutbound: 15, InternalOutbound: 16, DraftOutbound: 17 };

var checkedAll = false;
var DeletedAttach = "";
//sessionStorage.setItem('CurrentTypeView', 'Card');

$(document).ready(function () {
    $('#selectAll').off().on('change', function () {
        if (!checkedAll) {
            $('.boxes input:checkbox').prop('checked', true);
            unfillSelectedAll();
            fillSelectedAll();
            toggleMultiFunctionSidebar("open");
            //$(this).parent('label').find('.cr-icon').addClass('glyphicon-ok');
            checkedAll = true;
        } else {
            $('.boxes input:checkbox').prop('checked', false);
            $('#selectAll').prop('checked', false);
            unfillSelectedAll();
            toggleMultiFunctionSidebar("close");
            checkedAll = false;
        }

    });


    $('.toggle_sidebar a').on('click', function () {

        if ($('.main_sidebar').width() == 70) {
            $('.main_sidebar').css({ 'width': '240px' });
            if (!isMob()) {
                // $('.content_wrapper').css('margin-right', '280px');
                //handling the boxes right toggling when slide the main sidebar
                if ($('.boxes-right').length > 0) {
                    $('.boxes-right > .row > div').width($('.boxes-right > .row > div').width() - 30);
                }
            }
        } else {
            $('.main_sidebar').css({ 'width': '' });
            if (!isMob()) {
                $('.content_wrapper').css('margin-right', '');
                //handling the boxes right toggling when slide the main sidebar
                if ($('.boxes-right').length > 0) {
                    $('.boxes-right > .row > div').width($('.boxes-right > .row > div').width() + 30);
                }
            }
        }
        //take care of slider resizing
        setTimeout(function () {
            if ($('.slick-initialized').length > 0 && $('.multi-function-slider').length > 0) {
                //$('.multi-function-slider').slick('unslick');
                initMultiFunctionSlider();
            }
        }, 305);
    });


    $('.toggle_sidebar_mobile a').on('click', function () {
        if ($('.main_sidebar').width() == 0) {
            $('.main_sidebar').css({ 'width': '240px' });
            $('.toggle_sidebar_mobile i').toggleClass('icon_close');
        } else {
            $('.main_sidebar').css({ 'width': '' });
            $('.toggle_sidebar_mobile i').toggleClass('icon_close');
        }
    });

    $('.toggling-menu > a').on('click', function () {
        $('.toggling-menu > a.active').removeClass('active');
        $(this).addClass('active');
    });

    $('.date-icon').on('click', function () {
        $(this).parent('div').find('input.form-date').focus();
    });

    var previewCanvas;

    $(document).click(function (e) {
        $menu = $('.top-nav-list--userdate');
        if (!$menu.is(e.target) // if the target of the click isn't the container...
            && $menu.has(e.target).length === 0) // ... nor a descendant of the container
        {
            $('.top-nav-list--userdate').removeClass('toggling');
            $('.dropdown-menu--special').fadeOut(0);
        }
    });

    $(document).ready(function () {

        //call function to make sidebar scrollable
        scrollableSidebar();

        //matching those classes to give free text sizing
        //$('.col-xs-4,.col-xs-6,.col-sm-4').matchHeight();

        $('.top-nav-list--userdate .inline-dropdown').on('click', function (e) {
            e.preventDefault();
            $('.top-nav-list--userdate').toggleClass('toggling');
            $('.dropdown-menu--special').fadeToggle(0);

        });

        //select2 init
        //if ($('.select2').length > 0) {
        //    $('.select2').select2({
        //        minimumResultsForSearch: 5,
        //        placeholder: function () {
        //            $(this).data('placeholder');
        //        },
        //        "language": {
        //            "noResults": function () {
        //                return "لا يوجد نتائج";
        //            }
        //        },
        //        width: '100%'
        //    });
        //}

        $('select[multiple]').on('change', function () {
            $.fn.matchHeight._update();

        });
        $('b[role="presentation"]').hide();
        $('.select2-selection__arrow').append('<i class="uicon icon_arrow_down"></i>');


        if ($('.form-date').length > 0) {
            $('.form-date').datetimepicker({
                locale: 'ar'
            });
        }




        if ($('.eye-seperator').length > 0) {
            $('.eye-seperator').css('min-height', $(document).height() - $('.eye-seperator').offset().top);
        }

        var allwidth = 0;
        if (isMob()) {
            if ($('.mob-overflow > .row').length > 0) {
                $('.mob-overflow > .row > div').each(function () {
                    allwidth = $(this).outerWidth() + allwidth + 15;
                });
                $('.mob-overflow > .row').width(allwidth);
            }
        }
        if ($('.previewBox').length > 0) {
            //Call the viewr
            //viwer options link " https://www.jqueryscript.net/rotator/jQuery-Plugin-for-Image-Zoom-Rotation-Plugin-iviewer.html "
            var iv1 = $("#viewer").iviewer({
                src: "images/doc_sample.jpg",
                update_on_resize: true,
                zoom_animation: true,
                mousewheel: true,
                onMouseMove: function (ev, coords) { },
                onStartDrag: function (ev, coords) { return true; }, //if false this image will not be dragged
                onDrag: function (ev, coords) { }
            });


            $(".VzoomIn").click(function () { iv1.iviewer('zoom_by', 1); });
            $(".VzoomOut").click(function () { iv1.iviewer('zoom_by', -1); });
            $("#fit").click(function () { iv1.iviewer('fit'); });
            $("#orig").click(function () { iv1.iviewer('set_zoom', 100); });
            $("#update").click(function () { iv1.iviewer('update_container_info'); });
            $(".iviewer_rotate_right").click(function () { iv1.iviewer('angle', 90); });
            $(".iviewer_rotate_left").click(function () { iv1.iviewer('angle', -90); });

            //call another image
            //Following only an sample of loading another image in integration this could be ajax request or other
            $(".ViewrPrev").click(function () {
                iv1.iviewer('loadImage', "images/doc_sample.jpg");
                $('.count').text('1');
                return false;
            });
            $(".ViewrNext").click(function () {
                iv1.iviewer('loadImage', "images/document2.png");
                $('.count').text('2');
                return false;
            });
            /*************#END# of Viewr *************** */

        }
        /**************Multi functions slider init************ */
        if ($('.multi-function-slider').length > 0) {
            initMultiFunctionSlider()
        }
        /**************#END#Multi functions slider init************ */


        /******take care of uploading files **/
        $(document).on('click', '.file .remove-mini', function () {
            var name = $(this).parent().find('.attachment_name').html();
            $(this).parent('div.file').remove();
            if ($('div.file').length == 0) {
            }
            DeletedAttach += name + ',';
        });

        $('.nav-filtering>li>a').on('click', function (e) {
            $('.nav-filtering>li.active').removeClass('active');
            $(this).parent('li').addClass('active');
            /**DEMO USE **/
            $('.boxes').parent('div').css('height', $('.boxes').parent('div').height())
            $('.boxes').fadeOut(150);
            setTimeout(function () {
                $('.boxes').fadeIn(200);
                $('.boxes').parent('div').css('height', '')
            }, 300);
            return false;
        });
    });

    $(window).on('resize', function () {
        if ($(window).width() != WindowWidth && $(window).height() != Windowheight) {
            //reinit slimscroll
            $('.main_sidebar ul').slimScroll({ destroy: true });
            $('.main_sidebar ul').css({ "overflow": "", "width": "", "height": "" });

            scrollableSidebar();

            //take care of right boxes on resize
            if ($('.boxes-right').length > 0) {
                $('.boxes-right > .row > div').css('width', '');
            }

            //take care of multi functios  slider (reinit)
            if ($('.slick-initialized').length > 0 && $('.multi-function-slider').length > 0) {
                //$('.multi-function-slider').slick('unslick');
                setTimeout(function () {
                    initMultiFunctionSlider();
                }, 305)

            }

        }//end ceck if height changed

    });
});

function isMob() {
    return $(window).width() < 767;
}

function showname() {
    var name = document.getElementById('Files');
    $('.uploaded-files').children('.newAdded').remove();
    var fileExist = false;
    for (i = 0; i < name.files.length; i++) {
        $('.uploaded-files').find('.attachment_name').each(function () {

            var S = $(this).html();
            if (S.includes(name.files.item(i).name)) {
                fileExist = true;
            }
        });
        if (!fileExist) {
            $('.uploaded-files').append('<div class="file newAdded"><i class="uicon icon_attachments"></i><span class="attachment_name">' + name.files.item(i).name + '</span><a href="javascript:void(0)" class="color-danger remove-mini"><i class="uicon icon_recycle"></i></a></div>');
        }
    }
    $('.col-uploadeed-files').fadeIn(200);
};

function shownameForTask() {
    var name = document.getElementById('Files');
    //$('.uploaded-files').children('.newAdded').remove();
    var fileExist = false;
    for (i = 0; i < name.files.length; i++) {
        $('.uploaded-files').find('.attachment_name').each(function () {

            var S = $(this).html();
            if (S.indexOf(name.files.item(i).name) > -1) {
                fileExist = true;
            }
        });
        if (!fileExist) {
            $('.uploaded-files').append('<div class="file newAdded"><i class="uicon icon_attachments"></i><span class="attachment_name">' + name.files.item(i).name + '</span><a href="javascript:removeTaskAttachemntPhysically()" class="color-danger remove-mini"><i class="uicon icon_recycle"></i></a></div>');
        }
    }
    $('.col-uploadeed-files').fadeIn(200);
};

function initMultiFunctionSlider() {
    $('.multi-function-slider').css("width", $('.multi-function-slider').parents('.expanded-container').width() - 60);

    //Bug:1953 - Uncaught TypeError: Cannot read property 'add' of null
    //https://github.com/kenwheeler/slick/issues/1953
    //http://jsfiddle.net/fmo50w7n/372/
    $('.multi-function-slider').not('.slick-initialized').slick({
        centerMode: false,
        slidesToShow: 6,
        arrows: true,
        rtl: true,
        infinite: false,
        prevArrow: '<i class="uicon icon_arrow_right"></i>',
        nextArrow: '<i class="uicon icon_arrow_left"></i>',
        responsive: [
            {
                breakpoint: 1340,
                settings: {
                    slidesToShow: 4
                }
            },
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 3
                }
            }
        ]
    });


}

function scrollableSidebar() {
    if ($('.main_sidebar').length == 0) {
        //remove sidebar toggle in mobile if no sidebar is used
        $('.toggle_sidebar_mobile').remove()
    }

    if ($('.main_sidebar').length > 0) {

        if ($('.sub-pages-list').height() + $('.fixed-top').height() + $('.toggle_sidebar').height() > $(window).height()) {
            if (!isMob()) {
                $('.main_sidebar ul').slimScroll({
                    height: $(window).height() - ($('.fixed-top').height() + $('.toggle_sidebar').height()),
                    railVisible: false
                });
            } else {
                $('.main_sidebar ul').slimScroll({
                    height: $(window).height() - ($('.fixed-top').height()),
                    railVisible: false
                });
            }

        }
    }
}

function closeSearch() {
    $('#searchOverlay').fadeOut(200);
    $('body,html').css({ "overflow": "", "height": "" });
}
function openSearch() {
    $('#searchOverlay').fadeIn(200);
    $('body,html').css({ "overflow": "hidden", "height": "100%" });

}
$('#searchOverlay').on('click', function (e) {
    if (e.target !== this)
        return;
    closeSearch();
});

function validateSearch() {
    if ($.trim($('.searchKeyword').val()) == "") {
        //$('.searchKeyword').css('border','1px solid #AE0000');
        $('.searchKeyword').focus();
        return false;
    }
}

function toggleBoxesRight() {

    //$('.boxes-right > .row > div').width($('.boxes-right > .row > div').width());
    $('.boxes-right').toggleClass('hidding_box');
    if ($('.boxes-right').hasClass('hidding_box')) {
        $('.boardered-eye .icon_eye').hide();
        $('.boardered-eye .fa-eye-slash').removeClass('hidden_');
        $('.boardered-eye .fa-eye-slash').show();
        $('.expanded-container').addClass('col-md-11 col-sm-11');

    } else {
        $('.boardered-eye .fa-eye-slash').hide();
        $('.boardered-eye .icon_eye').show();

        $('.expanded-container').addClass('col-md-8 col-sm-7');
        $('.expanded-container').removeClass('col-md-11 col-sm-11');

    }
    //take care of slider resizing
    setTimeout(function () {
        if ($('.slick-initialized').length > 0 && $('.multi-function-slider').length > 0) {
            //$('.multi-function-slider').slick('unslick');
            initMultiFunctionSlider();
        }
    }, 305);

}

function ImagetoPrint(source) {
    return "<html><head><script>function step1(){\n" +
        "setTimeout('step2()', 10);}\n" +
        "function step2(){window.print();window.close()}\n" +
        "</scri" + "pt></head><body onload='step1()'>\n" +
        "<img src='" + source + "' /></body></html>";
}

function PrintImage(source) {
    Pagelink = "about:blank";
    var pwa = window.open(Pagelink, "_new");
    pwa.document.open();
    pwa.document.write(ImagetoPrint(source));
    pwa.document.close();
}

function goTableView(event) {
    //localStorage.setItem('CurrentTypeView', 'Table');
    //$('#TransactionCards').attr('style', 'display: none !important');
    //$('#TransactionTable').attr('style', 'display: flex !important');
    //$('.boxes').addClass('table-view');
    //$('.boxes').removeClass('card-view');
    //$('.toggling-menu .icon_table').parent('a').addClass('active');
    //$('.toggling-menu .icon_cards').parent('a').removeClass('active');
    //if (event) {
    //    var type = $(event.currentTarget).data('type');
    //    localStorage.setItem('CurrentTypeView', type);
    //}
}

function goCardsView(event) {
    //sessionStorage.setItem('CurrentTypeView', 'Card');
    //$('#TransactionTable').attr('style', 'display: none !important');
    //$('#TransactionCards').attr('style', 'display: flex !important');
    //$('.boxes').addClass('card-view');
    //$('.boxes').removeClass('table-view');
    //$('.toggling-menu .icon_table').parent('a').removeClass('active');
    //$('.toggling-menu .icon_cards').parent('a').addClass('active');
    //if (event) {
    //    var type = $(event.currentTarget).data('type');
    //    localStorage.setItem('CurrentTypeView', type);
    //}
}

function goToViewAfterPaging() {
    var type = localStorage.getItem('CurrentTypeView');
    if (type) {
        if (type === "Table") {
            goTableView();
        }
        else if (type === "Card") {
            goCardsView();
        }
    }
}

function ResetViewPage() {
    if (currentView) {
        var type = $(currentView).data('type');
        if (type == 'Table') {
            goTableView();
        }
        else {
            goCardsView();
        }
    }
    currentView = undefined;
}

function toggleFilterBox() {
    $('.options a.filled').removeClass('filled');
    $('.options .icon_filter').parent('a').toggleClass('filled');
    $('.order-box').hide();
    $('.filter-box').slideToggle(200);
}

function toggleOrderBox() {
    $('.options a.filled').removeClass('filled');
    $('.options .icon_order').parent('a').toggleClass('filled');
    $('.filter-box').hide();
    $('.order-box').slideToggle(200);
}

//Validation example FOR form-horizontal
function validateForm(el) {

    var isValid = true;
    $('.validation-error').remove();
    $(el).find("input,select, textarea").each(function () {
        if (typeof ($(this).attr("require")) != "undefined" && $.trim($(this).val()) == "") {
            $(this).addClass('invalid-input');
            $(this).parent('div').append("<span class='validation-error'><i class='glyphicon glyphicon-exclamation-sign'></i>" + $(this).attr('data-validation-message') + "</span>");
            isValid = false;
        } else if (typeof ($(this).attr("require")) != "undefined" && $.trim($(this).val()) != "") {
            $(this).removeClass('invalid-input');

        }
    });

    return isValid;
}

/************************* #BOTTOM TOGGLING FUNCTIONS# ********************** */

function toggleMultiFunctionSidebar(action) {
    if ($('.sidebarMultiOptions').length > 0) {
        var selectedCount = $('.boxes input:checked').length;
        if (action == 'close') {
            $('.paper_count').text(selectedCount);
            $('.sidebarMultiOptions:not(.invisible)').addClass('invisible');
        } else {
            $('.paper_count').text(selectedCount);
            $('.sidebarMultiOptions').removeClass('invisible');
        }

    }
}



function toggleSelectedPaperBox() {
    $('.selected-papers-wrapper').slideToggle(160);
    $('.info-bottom .close-box').toggleClass('transform');

}

$(document).on('click', '.papers-selected li >a.remove', function () {
    $(this).parents('li').remove();

    //make selection of thetargeted box false as we remove it here
    $('div[data-id=' + $(this).attr('data-id') + ']').find('input:checkbox').prop('checked', false);

    $('#selectAll').prop('checked', false);
    var count = $('.boxes input:checked').length
    $('.paper_count').text(count);
    checkedAll = false;
    //close the box whre no selected boxes lifted
    if ($('.papers-selected li').length == 0) {
        toggleSelectedPaperBox();
        toggleMultiFunctionSidebar("close");
    }
})

function fillSelectedList(id) {
    //id param is teh id of the box to conect teh selected bbox with the bottom bar
    //----> the id here is the same of selected box (original box)
    $('ul.papers-selected').append('<li><a class="remove" data-id="' + id + '" href="#"><i class="uicon icon_close"></i></a><span class="paper-name">' + $('#entityName_' + id).text() + '</span><span class="paper-number">' + $('#Number_' + id).text() + '</span></li>');
}


function fillSelectedAll() {
    //we take all the boxes and append it to list
    $('ul.papers-selected').html("");
    $('.boxes .box').each(function () {
        $('ul.papers-selected').append('<li><a class="remove" data-id="' + $(this).parent('div').attr('data-id') + '" href="#"><i class="uicon icon_close"></i></a><span class="paper-name">' + $('#entityName_' + $(this).parent('div').attr('data-id')).text() + '</span><span class="paper-number">' + $('#Number_' + $(this).parent('div').attr('data-id')).text() + '</span></li>');
    });

}
function unfillSelectedList(id) {
    //id param is teh id of the box to conect teh selected bbox with the bottom bar
    //----> the id here is the same of selected box (original box)

    $('ul.papers-selected li a.remove[data-id=' + id + ']').parent('li').remove();


}

//$('.boxes input:checkbox').on('change', function () {
//    if ($('.boxes input:checked').length > 0) {
//        toggleMultiFunctionSidebar("open");
//        fillSelectedList($(this).parents('.box').parent('div').attr('data-id'));
//    } else {
//        toggleMultiFunctionSidebar("close");
//    }

function onCheckBoxClicked(chk) {
    if ($('.boxes input:checked').length > 0) {
        if ($('.boxes input:checked').length == $('.boxes input:checkbox').length) {
            $('#selectAll').click();
            return;
        }
        toggleMultiFunctionSidebar("open");
        fillSelectedList($(chk).parents('.box').parent('div').attr('data-id'));
    } else {
        toggleMultiFunctionSidebar("close");
    }

    if (!$(chk).is(':checked')) {
        unfillSelectedList($(chk).parents('.box').parent('div').attr('data-id'));
        $('#selectAll').prop('checked', false);
        checkedAll = false;
    }

    if ($('.boxes input:checkbox:checked').length == $('.boxes input').length) {
        $('#selectAll').prop('checked', true);
        checkedAll = true;
    }
}

function unfillSelectedAll() {
    $('ul.papers-selected').html("");
    toggleMultiFunctionSidebar("close");
}

function paperSendToDone() {
    $('.msg-confirm-box').slideToggle(260);
    $('.selected-papers-wrapper').hide(0);
    $('.info-bottom a.close-box').css({ 'opacity': '0', 'pointer-events': 'none' });
    $('.bottom-actions > div').fadeOut(100);
}
function closePaperSendToDone() {
    $('.msg-confirm-box').hide();
    $('.selected-papers-wrapper').slideDown(260);
    $('.info-bottom a.close-box').css({ 'opacity': '', 'pointer-events': '' });
    $('.bottom-actions > div').fadeIn(100);
}

function sendActionShowMessage() {
    $('.success-done-msg').slideToggle(260);
    $('.msg-confirm-box').hide(0);
    $('.bottom-actions > a.hidden-success').fadeIn(100);
}

function closeSendActionShowMessage() {
    $('.success-done-msg').hide();
    $('.selected-papers-wrapper').hide(0);
    $('.msg-confirm-box').slideUp(100);
    $('.bottom-actions > a.hidden-success').fadeOut(100);
    $('.bottom-actions > div').fadeIn(100);
    $('.info-bottom a.close-box').css({ 'opacity': '', 'pointer-events': '' });
    $('.bottom-actions > a.hidden-success').hide();
}

function showCalender(calenderName) {
    $('#' + calenderName).trigger('focus');
}
