using Amazon.Scheduler;
using Amazon.Scheduler.Model;
using Newtonsoft.Json;

public class AppointmentService
{
    private readonly IAmazonScheduler _schedulerClient;

    public AppointmentService(IAmazonScheduler schedulerClient)
    {
        _schedulerClient = schedulerClient;
    }

    public async Task CreateOneTimeReminder(string appointmentId, DateTime reminderTime, string phoneNumber, string appointmentTime)
    {
        var createScheduleRequest = new CreateScheduleRequest
        {
            Name = $"{appointmentId}_{reminderTime.Ticks}",
            ScheduleExpression = $"at({reminderTime:yyyy-MM-ddTHH:mm:ss})",
            ScheduleExpressionTimezone = "America/Toronto",
            State = ScheduleState.ENABLED,
            FlexibleTimeWindow = new FlexibleTimeWindow { Mode = FlexibleTimeWindowMode.OFF },
            ActionAfterCompletion = ActionAfterCompletion.DELETE,
            Target = new Target
            {
                Arn = "arn:aws:lambda:ca-central-1:476114134169:function:AppointmentReminder",
                RoleArn = "arn:aws:iam::476114134169:role/service-role/Amazon_EventBridge_Scheduler_LAMBDA_6eedb9af2a",
                Input = JsonConvert.SerializeObject(new
                {
                    phone_number = phoneNumber,
                    appointment_time = appointmentTime
                })
            }
        };

        await _schedulerClient.CreateScheduleAsync(createScheduleRequest);
    }
}
