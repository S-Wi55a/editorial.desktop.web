/**
* Modal - reusable modal component
* @module
* @param {object} config - the initialisation config
*/

class Modal {

    //_body = Object;
    //_scope = Object;
    //_modalBg = Object;
    //_modalContent = Object;
    //_closeModal = Object;
    //_loading = Object;

    //_isActive = false;

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

    updateView(html) {

        $(this._modalContent).html(html);

        this._modalContent.width

        let ajaxEvt = document.createEvent('Event')
        ajaxEvt.initEvent('ajax-completed', true, true)
        ajaxEvt.Response = this._scope;
        window.dispatchEvent(ajaxEvt);

        this._loading.removeAttribute('data-is-active')
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

        $.ajax({
            url: url,
            type: 'GET',
            success: function (response) {
                $this.updateView(response);
            }
        });
    }

    addModalTriggerEvents(container)
    {
        var $this = this;
        // go through each modal trigger element and show the modal on click
        container.querySelectorAll('[data-modal-trigger]').forEach((item) => {
            item.addEventListener('click', function (e) {
                e.preventDefault();
                $this.showModal(this);
            });
        })
    }
}


export let modal = new Modal();
