using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveToGoalAgent : Agent
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Material win;
    [SerializeField] private Material lose;
    [SerializeField] private MeshRenderer mesh;
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-11f, 11f), 0, Random.Range(-11f, 11f));
        targetTransform.localPosition = new Vector3(Random.Range(-11f, 11f), 0, Random.Range(-11f, 11f));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        continuousAction[0] = Input.GetAxisRaw("Horizontal");
        continuousAction[1] = Input.GetAxisRaw("Vertical"); 
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag.Equals("Wall")) {
            SetReward(-1f);
            mesh.material = lose;
            EndEpisode();
        } else if (collider.tag.Equals("target")) {
            SetReward(1f);
            mesh.material = win;
            EndEpisode();
        }
        
    }
}
