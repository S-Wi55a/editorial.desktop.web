/**
* Modal - reusable modal component
* @module
* @param {object} config - the initialisation config
*/

class Modal {

    constructor() {

        var $this = this;

        this._body = document.querySelector('body')
        this._scope = document.querySelector('[data-ajax-modal]');
        this._modalContent = this._scope.querySelector('._c-modal__inner');
        this._closeModal = this._scope.querySelector('._c-modal__close');
        this._loading = this._scope.querySelector('._c-spinner');

        this._isActive = false;

        this.addModalTriggerEvents(document);
        window.addEventListener('ajax-completed', function (e) { $this.addModalTriggerEvents(e.Response); });
        window.addEventListener('close-modal', function () { $this.closeModal(); });

        this._scope.addEventListener('click', function (event) {
            if (event.target.className.includes('_c-modal') || event.target.className == '_c-modal__close') {
                $this.closeModal()
            }
        })
        this._closeModal.addEventListener('click', function () {
            $this.closeModal()
        })
        document.addEventListener('keyup', function(e) { $this.modalKeyPressed(e) })

    }

    closeModal()
    {
        this._scope.setAttribute('data-is-active', 'false');
        this._body.setAttribute('data-is-locked', 'false');
        this._modalContent.innerHTML = ''
        this._isActive = false;
    }

    modalKeyPressed(event)
    {
        if (this._isActive && event.keyCode === 27) {
            this.closeModal();
        }
    }

    showModal(element)
    {
        let modalType = element.getAttribute('data-modal-type');
        let url = element.getAttribute('href');

        if (element.getAttribute('data-ajax-url')) {
            url = element.getAttribute('data-ajax-url')
        }

        this.open(url, modalType);
    }

    updateView(html, index) {

        $(this._modalContent).html(html); //can be changed to appendchild

        // HACK: needs to be here for index, should find better solution
        document.querySelector('._c-slideshow--modal').setAttribute('data-slideshow-start', index);


        let ajaxEvt = document.createEvent('Event')
        ajaxEvt.initEvent('ajax-completed', true, true)
        ajaxEvt.Response = this._scope;
        window.dispatchEvent(ajaxEvt);

        this._loading.removeAttribute('data-is-active');

    }


    open(url, modalType)
    {
        this._isActive = true;
        modalType ? this._scope.className = '_c-modal _c-modal--' + modalType : this._scope.className = '_c-modal';
        this._body.setAttribute('data-is-locked', 'true');
        this._scope.setAttribute('data-is-active', 'true');
        this._loading.setAttribute('data-is-active', 'true')
        this.loadModalData(url)
    }

    loadModalData(url) {

        var $this = this;

        //var request = new XMLHttpRequest();
        //request.open('GET', url , true);

        //request.onload = function() {
        //    if (request.status >= 200 && request.status < 400) {
        //        var resp = request.responseText;
        //        $this.updateView(resp);
        //    } else {
        //        // We reached our target server, but it returned an error
        //    }
        //};

        //request.send();


        //var load_types = document.querySelector('[data-modal-data]')[0];
        //var div = document.createElement("div");
        //div.innerHTML = load_types;

        let index = this.getQueryVariable(url, "imageIndex")
        let modalData = document.querySelector('[data-modal-data]')
        let dataString = "";

        //filter the comment data and return a string
        Array.prototype.filter.call(modalData.childNodes,function(el){
            return el.nodeType == 8;
        }).forEach(function(elem){
            dataString += elem.data
        });

        $this.updateView(dataString, index);


    }

    addModalTriggerEvents(container)
    {
        var $this = this;
        // go through each modal trigger element and show the modal on click
        var modalTrigger = container.querySelectorAll('[data-modal-trigger]');
        for (var item of modalTrigger) {
            item.addEventListener('click', function (e) {
                e.preventDefault();
                $this.showModal(this);
            });
        }
    }

    getQueryVariable(url, variable)
    {
        var query = url ? url.split('?')[1] : window.location.search.substring(1);
        var vars = query.split("&");
        for (var i=0;i<vars.length;i++) {
            var pair = vars[i].split("=");
            if(pair[0] == variable){return pair[1];}
        }
        return(false);
    }
}


export let modal = new Modal();
