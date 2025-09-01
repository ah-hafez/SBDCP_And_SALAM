$.validator.unobtrusive.adapters.add(
                'OrgStructureValidation',
                ['validationgroup'],
                function (options) {
                    
                    options.rules['OrgStructureValidation'] = {
                        validationgroup: options.params['validationgroup'],
                        ErrorMessage:options.message
                    };
                    options.messages['OrgStructureValidation'] = options.message;
                });

$.validator.addMethod('OrgStructureValidation',
    
    function (value, element, parameters) {
        
        var findRoot = true;
        var departmentsWithNoParent = '';

        if (value != '' && value != null) {
            
            var departmentArray = $.parseJSON(value);

            for (var i = 0; i < departmentArray.length; i++) {

                if (departmentArray[i].parentId == '' || departmentArray[i].DepName == '') {

                    departmentsWithNoParent +='\n' + departmentArray[i].text;
                }

                if (departmentArray[i].parentId == 'root') {

                    findRoot = false ;
                }
            }

            if (departmentsWithNoParent != '') {

                //$.validator.messages.OrgStructureValidation = parameters.ErrorMessage + departmentsWithNoParent;
                return false
            }

            if (!findRoot) {

                //$.validator.messages.OrgStructureValidation = 'error' + departmentsWithNoParent;
                return false
            }
        }
        return true;
    }
);