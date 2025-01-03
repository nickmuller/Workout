"use strict";

window.workout = window.workout || {};

workout.say = function (message) {
    const msg = new SpeechSynthesisUtterance();
    msg.text = message;
    msg.lang = "nl-NL";
    window.speechSynthesis.speak(msg);
};