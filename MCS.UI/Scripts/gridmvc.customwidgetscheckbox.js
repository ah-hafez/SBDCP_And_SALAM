
function CustomCheckBoxFilterWidget(dataJson) {
 
    this.getAssociatedTypes = function () {
        return ["CheckBoxFilterWidget"];
    };
   
    this.onShow = function () {
       
    };

    this.showClearFilterButton = function () {
        return true;
    };
   
    this.onRender = function (container, lang, typeName, values, cb, data) {
        this.cb = cb;
        this.container = container;
        this.lang = lang;

        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };
        var test = JSON.parse(dataJson.replace(/&quot;/g, '"'));
        

        this.renderWidget(test); 
        this.registerEvents();
    };
    this.renderWidget = function (test) {
        var html = '';
        var a = Number(this.value.filterValue);

        $.each(test, function (key, data) {

            var x = a | data.Id;

            if (x == a) {
                html += "<div class='form-group'><div class='col-md-4'><label class='check'><input type='checkbox' checked class='checkLookup' data-id='" + data.Id + "'><label style = 'padding-right:5px'>" + data.Text + "</label></label></div></div>";
            }
            else {
                html += "<div class='form-group'><div class='col-md-4'><label class='check'><input type='checkbox' class='checkLookup' data-id='" + data.Id + "'><label style = 'padding-right:5px'>" + data.Text + "</label></label></div></div>";
            }
        })


        html += '<div class="grid-filter-buttons">\
                        <button type="button" class="btn btn-primary filterLookup">' + this.lang.applyFilterButtonText + '</button>\
                    </div>';

        this.container.append(html);
    };
  
    this.registerEvents = function () {
        var filterLookup = this.container.find(".filterLookup");

        var $context = this;

        filterLookup.click(function () {
            var values = [];
            var bitval = 0;
            var hasChecked = false;
            $(".checkLookup:checked").each(function () {
                hasChecked = true;
                bitval = Number(bitval) ^ $(this).data("id");
            })

            if (hasChecked) {
                values.push({ filterValue: bitval, filterType: 1 /* Equals */ });
            }

            $context.cb(values);
        });
    };

}