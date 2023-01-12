using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace ConferenceCallTwilioCore.Controllers;

public class VoiceController : TwilioController
{
    private readonly ILogger<VoiceController> _log;

    public VoiceController(ILogger<VoiceController> logger)
    {
        _log = logger;
    }

    [HttpPost("/Twilio/IncomingCall")]
    public IActionResult OnIncomingCall([FromForm] string From)
    {
        _log.LogInformation("Incoming call from {@Caller}.", From);

        string moderator = "+1234567890";
        try
        {
            VoiceResponse response = new();
            Dial dial = new();

            // If the caller is our MODERATOR, then start the conference when they
            // join and end the conference when they leave
            if (From == moderator)
            {
                dial.Conference("My conference",
                                startConferenceOnEnter: true,
                                endConferenceOnExit: true);
            }
            else
            {
                // Otherwise have the caller join as a regular participant
                dial.Conference("My conference",
                                startConferenceOnEnter: false);
            }

            response.Append(dial);

            return TwiML(response);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
