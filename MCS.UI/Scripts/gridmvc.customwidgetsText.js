/***
* This script demonstrates how you can build you own custom filter widgets:
* 1. Specify widget type for column:
*       columns.Add(o => o.Customers.CompanyName)
*           .SetFilterWidgetType("CustomCompanyNameFilterWidget")
* 2. Register script with custom widget on the page:
*       <script src="@Url.Content("~/Scripts/gridmvc.customwidgets.js")" type="text/javascript"> </script>
* 3. Register your own widget in Grid.Mvc:
*       GridMvc.addFilterWidget(new CustomersFilterWidget());
*
* For more documentation see: http://gridmvc.codeplex.com/documentation
*/

/***
* CustomersFilterWidget - Provides filter user interface for customer name column in this project
* This widget onRenders select list with avaliable customers.
*/

function CustomTextFilterWidget() {
    /***
    * This method must return type of registered widget type in 'SetFilterWidgetType' method
    */
    this.getAssociatedTypes = function () {
        return ["TextFilterWidget"];
    };
    /***
    * This method invokes when filter widget was shown on the page
    */
    this.onShow = function () {
        /* Place your on show logic here */
    };

    this.showClearFilterButton = function () {
        return true;
    };
    /***
    * This method will invoke when user was clicked on filter button.
    * container - html element, which must contain widget layout;
    * lang - current language settings;
    * typeName - current column type (if widget assign to multipile types, see: getAssociatedTypes);
    * values - current filter values. Array of objects [{filterValue: '', filterType:'1'}];
    * cb - callback function that must invoked when user want to filter this column. Widget must pass filter type and filter value.
    * data - widget data passed from the server
    */
    this.onRender = function (container, lang, typeName, values, cb, data) {
        //store parameters:
        this.cb = cb;
        this.container = container;
        this.lang = lang;

        //this filterwidget demo supports only 1 filter value for column column
        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };

        this.renderWidget(); //onRender filter widget
        this.fillText($.parseJSON(data));
        this.registerEvents(); //handle events
    };
    this.renderWidget = function () {
        var html = '<p><i>This is custom filter widget demo.</i></p>\
                    <p>Select customer to filter:</p>\
                    <select style="width:250px;" class="grid-filter-type list form-control">\
                    </select>';
        this.container.append(html);
    };
    this.fillText = function (items) {
        var List = this.container.find(".list");

        var index = List.parents('th').index();
        var table = List.parents('table');
        if (items == null) {
            $("td:nth-child(" + (index + 1) + ")").each(function (i, el) {
                var x = true
                List.children('option').each(function (j, option) {
                    
                    if (option.innerText==el.firstChild.textContent)
                    {
                        x = false;
                    }
                });
                if (x == true) {
                    List.append('<option value="' + el.firstChild.textContent + '">' + el.firstChild.textContent + '</option>');
                }
            });
        }
        else{
        for (var i = 0; i < items.length; i++) {
            List.append('<option ' + (items[i] == this.value.filterValue ? 'selected="selected"' : '') + ' value="' + items[i] + '">' + items[i] + '</option>');
        }
        }
    };
    /***
    * Internal method that register event handlers for 'apply' button.
    */
    this.registerEvents = function () {
        var List = this.container.find(".list");
        //save current context:
        var $context = this;
        //register onclick event handler
        List.change(function () {
            //invoke callback with selected filter values:
            var values = [{ filterValue: $(this).val(), filterType: 1 /* Equals */ }];
            $context.cb(values);
        });
    };

}